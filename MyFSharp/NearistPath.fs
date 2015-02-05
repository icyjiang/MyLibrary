namespace FSharp.NearistPath

//type Station =
//| A 
//| B 
//| C 
//| D 
//| E 
//| F 
//| G 
//| H 
//| I 
//| J 
//| K 
//| L 
//| M 
//| N 
//| O 
//| P 
//| Q 
//| R 
//| S 
//| T 
//| U 
//| V 
//| W 
//| X 
//| Y 
//| Z 

type NearistPath() =
    //根据节点，获取关联的节点列表
    static member private GetContacts (lstContact:(int * int) seq) (key:int)  = 
        let res : int list ref = ref []
        for x,y in lstContact do
            if x = key && res.Value |> List.filter (fun a -> a = y) |> List.length = 0 then res := y :: res.Value 
            else if y = key && res.Value |> List.filter (fun a -> a = x) |> List.length = 0 then res := x :: res.Value 
        res.Value

    static member GetNearistPath(lstContact:(int * int) seq, from:int, target:int) = 
        NearistPath.GetNearistPath(lstContact, from, target, 20, 3)

    static member GetNearistPath(lstContact:(int * int) seq, from:int, target:int, maxLength:int, 
                                 moreRegion:int) = 
        let res: int list list ref = ref [[]]
        let minLength:int ref = ref maxLength

        //添加到结果
        let addToRes(path : int list) = 
            if path.Length <= (minLength.Value + moreRegion) then //如果path的长度大于最小值+3，则无需添加
                res := path :: res.Value
                if path.Length <= minLength.Value then minLength := path.Length//刷新最短值
                //移除长度大于最小值+3的路径 
                res := res.Value |> List.filter 
                    (fun x -> x.Length <= minLength.Value + moreRegion && x.Length > 0)

        let rec act (path:int list) (node:int) =
            let contacts = NearistPath.GetContacts lstContact node
            //如果是集合中已有的节点 或非终点且无子节点，返回
            if path |> List.exists (fun x -> x = node) || (contacts.Length = 0 && node <> target) then ()
            else
                let newPath =  List.append path [node]
                if node = target then addToRes newPath
                else
                    if newPath.Length < maxLength && newPath.Length < (minLength.Value + moreRegion) then
                        for item in contacts do act newPath item
        if from = target then addToRes [from]
        else act [] from
        res.Value

//let data = [A,B;A,C;A,F;B,E;B,D;C,E;C,D;E,F;D,F]
//let maxLength = 20
//let moreRegion = 3
//let resLength = 5
//let classss = (new NearistPath(data, maxLength, moreRegion, resLength)).GetNearistPath(A,F)

