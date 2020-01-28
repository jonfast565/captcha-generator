namespace CaptchaGenerator.Lib

open System
open System.Drawing
open System.Drawing.Imaging
open System.IO

type internal GraphicsBitmapWrapper(bitmap : Bitmap, graphics : Graphics) = 
    member this.bitmap : Bitmap = bitmap
    member this.graphics : Graphics = graphics
    member this.BitmapToByteArray() : byte array = 
        use stream = new MemoryStream()
        this.bitmap.Save(stream, ImageFormat.Png)
        let streamBytes = stream.ToArray()
        streamBytes
