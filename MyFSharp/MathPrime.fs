namespace MyFSharp
type MathPrime = 
    //判断是否质数
    static member IsPrime n =
        let rec check i =
            i > n/2 || (n % i <> 0 && check (i + 1))
        check 2
    //分解因式
    static member GetPrime num = 
        let rec toCalc n m  = 
            match n with
                | _ when n = 1 -> []
                | _ when n % m = 0 -> m::(toCalc (n/m) m)
                | _ -> (toCalc n (m + (if m >= 3 then 2 else 1)))
        toCalc num 2
    //获取所有的约数
    static member GetDivisors num = 
        let s = int (Operators.sqrt(float num))
        let rec toCalc m = 
            match m with
                | _ when m > s -> []
                | _ when num % m = 0 -> m::((num / m)::(toCalc (m+1)))
                | _ -> toCalc (m+1)
        List.sort (toCalc 1)