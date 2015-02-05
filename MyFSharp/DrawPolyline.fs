namespace FSharp

open System
open System.Drawing
open System.Collections.Generic

type Polyline() =
    //排序并去除重复
    static member private SortAndDistinct (points:PointF seq) (tolerance:float32) = 
        let tmp = points |> Seq.sortBy (fun a -> a.X)
        let res:PointF list ref = ref [tmp |> Seq.head]
        for n in 0..(Seq.length tmp)-1 do
            let last = res.Value.Item (res.Value.Length-1)
            let curr = Seq.nth n tmp
            if curr.X-last.X > tolerance then res := List.append res.Value [curr]
        res.Value

    //根据画布的大小+留白转换坐标点
    static member private ConvertPoints (points:PointF list) (width:int) (height:int) (space:int)=
        let minX = (points |> List.minBy (fun a -> a.X)).X
        let maxX = (points |> List.maxBy (fun a -> a.X)).X
        let minY = (points |> List.minBy (fun a -> a.Y)).Y
        let maxY = (points |> List.maxBy (fun a -> a.Y)).Y
        let scaleX = float32(width-2*space)/(maxX-minX)
        let scaleY = float32(height-2*space)/(maxY-minY)
        points |> List.map 
            (fun a -> new PointF(a.X*scaleX+float32(space),float32(height-2*space)-a.Y*scaleY+float32(space)))
    
    //画图
    static member GetImageFromPoints (points:PointF seq) (tolerance:float32) (width:int) (height:int) (space:int)= 
        let newPs = Polyline.ConvertPoints (Polyline.SortAndDistinct points tolerance) width height space
        let bitmap = new Bitmap(width,height)
        let g = Graphics.FromImage(bitmap)
        g.Clear(Color.White)
        g.DrawLines(Pens.Red, List.toArray newPs)
        for p in newPs do
            g.FillEllipse(Brushes.Red,(p.X-3.0f),p.Y-3.0f,6.0f,6.0f)
//        g.SmoothingMode <- Drawing2D.SmoothingMode.HighQuality
        bitmap

    //求点
    static member GetValueFromX (points:PointF seq) (tolerance:float32) (key:float32)=
        if points |> Seq.length=0 then failwith "No point!"
        let sortPoints = Polyline.SortAndDistinct(points) tolerance
        let one = sortPoints.Head
        let lst = sortPoints.Item (sortPoints.Length-1)
        //只有一个点，不是所求的点，报错
        if sortPoints.Length=1 && one.X<>key then failwith "Only one point which is not the right point."
        else
            if key < one.X then //点小于最小点，求延长线上的值
                let two = sortPoints.Item 1
                one.Y-(two.Y-one.Y)*(one.X-key)/(two.X-one.X)
            else if key > lst.X then //点大于最大点，
                let lastTwo = sortPoints.Item (sortPoints.Length-2)
                lst.Y+(lst.Y-lastTwo.Y)*(key-lst.X)/(lst.X-lastTwo.X)
            else //介于之间
                let pre:PointF ref =ref one
                let res:float32 ref =ref one.Y
                for n in sortPoints do
                    if(n.X = key) then res := n.Y
                    else if pre.Value.X<key && n.X>key then res := n.Y-(n.Y-pre.Value.Y)*(n.X-key)/(n.X-pre.Value.X)
                    pre := n
                !res
