namespace CaptchaGenerator.Lib

open System
open System.Drawing
open System.Drawing.Drawing2D
open System.Drawing.Imaging
open System.Drawing.Text

type internal CaptchaDimensions(captchaString: string, captchaConstraints : CaptchaConstraints) = 
    // constants    
    static let heightDivisor = 4
    static let heightMultiplier = 2.5f
    static let widthMultiplier = 1.5f
    // image dims
    member this.padding = captchaConstraints.minFontSize
    member this.height = int ((float32 captchaConstraints.maxFontSize) * heightMultiplier)
    member this.width = captchaString.Length * int ((float32 captchaConstraints.maxFontSize) * widthMultiplier)
    member this.widthSansPadding = float (this.width - this.padding)
    member this.captchaLengthFloat = float captchaString.Length
    member this.widthIncrement = int (this.widthSansPadding / this.captchaLengthFloat)
    // naive centering, beware
    member this.heightTranslateOffset = this.height / heightDivisor

    member this.GenerateGraphicsFromBlankBitmap() = 
        let bitmap = new Bitmap(this.width, this.height, PixelFormat.Format32bppRgb)
        let backgroundRectangle = 
            new RectangleF(0.0f, 0.0f, float32 this.width, float32 this.height)
        let randomBackgroundColorBrush = ColorUtil.GetRandomColorAsBrush()
        let graphics = Graphics.FromImage(bitmap)
        graphics.TextRenderingHint <- TextRenderingHint.AntiAliasGridFit
        graphics.InterpolationMode <- InterpolationMode.HighQualityBilinear;
        graphics.FillRectangle(randomBackgroundColorBrush, backgroundRectangle)
        new GraphicsBitmapWrapper(bitmap, graphics)