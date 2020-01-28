namespace CaptchaGenerator.Lib

open System
open System.Drawing
open System.Drawing.Imaging
open System.Drawing.Text
open System.IO
open System.Linq
open FSharp.Core

// TODO: Seriously, this code is full of primitive obsession. Will need to fix it
type CaptchaBuilder() = 
    
    static member private SetCaptchaCharInGraphics(character : char, mutableWidthCounter : int, 
                                                   w : GraphicsBitmapWrapper, captchaConstraints : CaptchaConstraints, 
                                                   captchaDimensions : CaptchaDimensions) = 
        let transformAngle = captchaConstraints.RandomAngleFromConstraints()
        let transformFontSize = captchaConstraints.RandomFontSizeFromConstraints()
        use currentFont = FontUtil.GetRandomFont(transformFontSize)
        w.graphics.TranslateTransform(float32 mutableWidthCounter, float32 captchaDimensions.heightTranslateOffset)
        w.graphics.RotateTransform(float32 transformAngle)
        w.graphics.DrawString(character.ToString(), currentFont, Brushes.Black, 0.0f, 0.0f)
        w.graphics.ResetTransform()
    
    static member private SetCaptchaRandomLinesInGraphics(captchaDimensions : CaptchaDimensions, 
                                                          captchaConstraints : CaptchaConstraints, 
                                                          w : GraphicsBitmapWrapper) = 
        let random = new Random(Guid.NewGuid().GetHashCode())
        let randomLineStartX = random.Next(0, captchaDimensions.width - 1)
        let randomLineStartY = random.Next(0, captchaDimensions.height - 1)
        let randomLineEndX = random.Next(0, captchaDimensions.width - 1)
        let randomLineEndY = random.Next(0, captchaDimensions.height - 1)
        let pen = new Pen(Color.LightGray, float32 captchaConstraints.randomLineWidth)
        w.graphics.DrawLine(pen, randomLineStartX, randomLineStartY, randomLineEndX, randomLineEndY)
    
    static member private WriteCaptchaLinesInternal(captchaDimensions : CaptchaDimensions, 
                                                    captchaConstraints : CaptchaConstraints, w : GraphicsBitmapWrapper) = 
        for i in 1..captchaConstraints.numberOfRandomLines do
            CaptchaBuilder.SetCaptchaRandomLinesInGraphics(captchaDimensions, captchaConstraints, w)
    
    static member private WriteCaptchaStringCharByCharInternal(captchaString : string, 
                                                               captchaConstraints : CaptchaConstraints, 
                                                               captchaDimensions : CaptchaDimensions, 
                                                               w : GraphicsBitmapWrapper) = 
        let mutable mutableWidthCounter = captchaDimensions.padding
        for character in captchaString do
            CaptchaBuilder.SetCaptchaCharInGraphics
                (character, mutableWidthCounter, w, captchaConstraints, captchaDimensions)
            mutableWidthCounter <- mutableWidthCounter + captchaDimensions.widthIncrement
    
    static member private DrawCaptchaBitmapAndGetResultInternal(captchaString : string, 
                                                                captchaConstraints : CaptchaConstraints, 
                                                                captchaDimensions : CaptchaDimensions) = 
        let graphicsInfo = captchaDimensions.GenerateGraphicsFromBlankBitmap()
        let mutable mutableWidthCounter = captchaDimensions.padding
        CaptchaBuilder.WriteCaptchaStringCharByCharInternal
            (captchaString, captchaConstraints, captchaDimensions, graphicsInfo)
        CaptchaBuilder.WriteCaptchaLinesInternal(captchaDimensions, captchaConstraints, graphicsInfo)
        let streamBytes = graphicsInfo.BitmapToByteArray()
        new CaptchaResult(captchaString, streamBytes)
    
    member private this.BuildCaptchaInternal(captchaString : string, captchaConstraints : CaptchaConstraints) = 
        captchaConstraints.TestCaptchaConstraints()
        let captchaDimensions = new CaptchaDimensions(captchaString, captchaConstraints)
        CaptchaBuilder.DrawCaptchaBitmapAndGetResultInternal(captchaString, captchaConstraints, captchaDimensions)
    
    static member private GenerateRandomCaptchaStringOfLength(numberOfDigits : int) : string = 
        let captchaStringGenerator = new CaptchaStringGenerator(numberOfDigits)
        captchaStringGenerator.GenerateString()
    
    member this.BuildCaptcha(numberOfDigits : int) : CaptchaResult = 
        if numberOfDigits < 0 then 
            raise 
            <| new CaptchaGeneratorException("Captcha digit count " + numberOfDigits.ToString() + " is less than 0")
        let randCaptchaString = CaptchaBuilder.GenerateRandomCaptchaStringOfLength(numberOfDigits)
        let defaultCaptchaConstraints = new CaptchaConstraints()
        this.BuildCaptchaInternal(randCaptchaString, defaultCaptchaConstraints)
    
    member this.BuildCaptcha(captchaString : string) : CaptchaResult = 
        let defaultCaptchaConstraints = new CaptchaConstraints()
        this.BuildCaptchaInternal(captchaString.ToUpperInvariant(), defaultCaptchaConstraints)
