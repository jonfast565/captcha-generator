namespace CaptchaGenerator.Lib

open System
open System.Linq
open System.Drawing

type internal FontUtil() =
    static let fontFamilies : string array = [| "Arial"; "Helvetica"; "Verdana"; "Calibri"; "Times New Roman" |]

    static member GetRandomFont(size: int) =
        let random = new Random()
        let randomFontFamilyIdx = random.Next(fontFamilies.Length)
        let randomFontFamily = fontFamilies.ElementAt(randomFontFamilyIdx)
        let currentFont = new Font(randomFontFamily, float32 size)
        currentFont
