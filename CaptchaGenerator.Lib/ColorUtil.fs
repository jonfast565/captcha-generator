namespace CaptchaGenerator.Lib

open System
open System.Linq
open System.Drawing

type internal ColorUtil() = 
    static let lighteningFactor = 0.9
    static member GetRandomColorAsBrush() = ColorUtil.GetRandomColor() |> ColorUtil.ColorToBrush
    static member private ColorToBrush(c : Color) = new SolidBrush(c)
    
    static member private InterpolateColors(color1 : Color, color2 : Color, percentage : float) : Color = 
        let a1 = (float color1.A) / 255.0
        let r1 = (float color1.R) / 255.0
        let g1 = (float color1.G) / 255.0
        let b1 = (float color1.B) / 255.0
        let a2 = (float color2.A) / 255.0
        let r2 = (float color2.R) / 255.0
        let g2 = (float color2.G) / 255.0
        let b2 = (float color2.B) / 255.0
        
        let getA3 = 
            try 
                Convert.ToByte((a1 + (a2 - a1) * percentage) * 255.0)
            with :? OverflowException -> 255uy
        
        let getR3 = 
            try 
                Convert.ToByte((r1 + (r2 - r1) * percentage) * 255.0)
            with :? OverflowException -> 255uy
        
        let getG3 = 
            try 
                Convert.ToByte((g1 + (g2 - g1) * percentage) * 255.0)
            with :? OverflowException -> 255uy
        
        let getB3 = 
            try 
                Convert.ToByte((b1 + (b2 - b1) * percentage) * 255.0)
            with :? OverflowException -> 255uy
        
        let a3Int = int getA3
        let r3Int = int getR3
        let b3Int = int getB3
        let g3Int = int getG3
        Color.FromArgb(a3Int, r3Int, g3Int, b3Int)
    
    static member private GetRandomColor() : Color = 
        let random = new Random()
        let knownColors : KnownColor array = Enum.GetValues(typedefof<KnownColor>).Cast<KnownColor>().ToArray()
        let randomKnownColor = knownColors.[random.Next(knownColors.Length)]
        let randomColor : Color = Color.FromKnownColor(randomKnownColor)
        let lightenedRandomColor = ColorUtil.InterpolateColors(randomColor, Color.White, lighteningFactor)
        lightenedRandomColor
