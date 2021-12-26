Imports System.Math
Imports System.Drawing.Drawing2D
Imports System.Drawing

Namespace DrawClasses
    Public Class Structures
        Public Structure GRect
            Dim X As Long
            Dim Y As Long
            Dim X1 As Long
            Dim Y1 As Long
        End Structure

        Public Structure GCirc
            Dim X As Long
            Dim Y As Long
            Dim Radius As Long
        End Structure

        Public Structure DPI_INfo
            Dim X As Long
            Dim Y As Long
        End Structure
    End Class

    Public Class Enums
        Public Enum Align
            LEFT = 0
            CENTER = 1
            RIGHT = 2
        End Enum

        Public Enum DestinationTypes
            BITMAP = 0
            JPEG = 1
            PNG = 2
            GIF = 3
            ICON = 4
        End Enum

        Enum BackGroundColor
            Red = 1
            Blue = 2
            Green = 3
            Orange = 4
            Yellow = 5
            Gray = 6
            White = 7
            Pink = 8
            NoColor = 9
        End Enum
    End Class

    Public Class Tools
        ''' <summary>
        ''' Calcula o raio de uma circunferencia com base na distancia entre 2 pontos
        ''' </summary>
        ''' <param name="X1">X inicial</param>
        ''' <param name="Y1">Y Inicial</param>
        ''' <param name="X2">X Final</param>
        ''' <param name="Y2">Y Final</param>
        ''' <returns>Metade do valor da distancia entre 2 pontos, em formato de numero inteiro</returns>
        Public Shared Function RadiusFromTriangle(X1 As Long, Y1 As Long, X2 As Long, Y2 As Long) As Long
            Return CLng(Round(Sqrt(Pow(Abs(X1 - X2), 2) + Pow(Abs(Y1 - Y2), 2)) / 2, 0))
        End Function

        ''' <summary>
        ''' Return Overlay value
        ''' </summary>
        ''' <param name="base"></param>
        ''' <param name="blend"></param>
        ''' <returns></returns>
        Public Shared Function OverlayMath(ByVal base As Integer, ByVal blend As Integer) As Integer
            Dim dbase As Double, dblend As Double

            dbase = CDbl(base / 255)
            dblend = CDbl(blend / 255)

            If dbase < 0.5 Then
                Return CInt((2 * dbase * dblend) * 255)
            Else
                Return CInt((1 - (2 * (1 - dbase) * (1 - dblend))) * 255)
            End If
        End Function

        ''' <summary>
        ''' Returns softlight values
        ''' </summary>
        ''' <param name="base"></param>
        ''' <param name="blend"></param>
        ''' <returns></returns>
        Public Shared Function SoftLightMath(ByVal base As Integer, ByVal blend As Integer) As Integer
            Dim dbase As Single
            Dim dblend As Single

            dbase = CLng(base / 255)
            dblend = CLng(blend / 255)

            If dblend < 0.5 Then
                Return CInt(((2 * dbase * dblend) + (dbase ^ 2) * (1 - (2 * dblend))) * 255)
            Else
                Return CInt(((Math.Sqrt(dbase) * (2 * dblend - 1)) + ((2 * dbase) * (1 - dblend))) * 255)
            End If
        End Function
    End Class

    Public Class Draw : Implements IDisposable

        Private Shared _G As Graphics

        Public ReadOnly DPI As Structures.DPI_INfo
        Private Shared Color As Color

        ''' <summary>
        ''' Initializes da class
        ''' </summary>
        ''' <param name="myGraphics"></param>
        ''' <param name="mColor"></param>
        ''' <param name="ComposeQuality"></param>
        ''' <param name="SmoothMode"></param>
        Public Sub New(myGraphics As Graphics,
               Optional mColor As Color = Nothing,
               Optional ComposeQuality As CompositingQuality = CompositingQuality.HighQuality,
               Optional SmoothMode As SmoothingMode = SmoothingMode.AntiAlias)

            _G = myGraphics
            _G.CompositingQuality = ComposeQuality
            _G.SmoothingMode = SmoothMode

            If mColor = Nothing Then
                Color = Color.Black
            Else
                Color = mColor
            End If
        End Sub

        ''' <summary>
        ''' This draw a Arc but with a center, radius, start angle and end angle. Instead of the usual rectangle from Graphics
        ''' </summary>
        ''' <param name="X"></param>
        ''' <param name="Y"></param>
        ''' <param name="Radius"></param>
        ''' <param name="EndAngle"></param>
        ''' <param name="StartAngle"></param>
        ''' <param name="xWidth"></param>
        Public Sub DrawArc(ByVal X As Integer, ByVal Y As Integer, ByVal Radius As Integer, ByVal EndAngle As Integer, Optional ByVal StartAngle As Integer = 0, Optional ByVal xWidth As Integer = 1)
            _G.DrawArc(New Pen(Color, xWidth), New Rectangle(X - Radius, Y - Radius, Radius * 2, Radius * 2), StartAngle, EndAngle)
        End Sub

        ''' <summary>
        ''' Draws a Bezier line with 4 given points
        ''' </summary>
        ''' <param name="X1"></param>
        ''' <param name="Y1"></param>
        ''' <param name="X2"></param>
        ''' <param name="Y2"></param>
        ''' <param name="X3"></param>
        ''' <param name="Y3"></param>
        ''' <param name="X4"></param>
        ''' <param name="Y4"></param>
        ''' <param name="xWidth"></param>
        Public Sub DrawBezier(ByVal X1 As Single, ByVal Y1 As Single, ByVal X2 As Single, ByVal Y2 As Single, ByVal X3 As Single, ByVal Y3 As Single, ByVal X4 As Single, ByVal Y4 As Single, Optional ByVal xWidth As Integer = 1)
            _G.DrawBezier(New Pen(Color, xWidth), X1, Y1, X2, Y2, X3, Y3, X4, Y4)
        End Sub

        ''' <summary>
        ''' Draw a circle with a center point and a radius
        ''' </summary>
        ''' <param name="X"></param>
        ''' <param name="Y"></param>
        ''' <param name="Radius"></param>
        ''' <param name="xWidth"></param>
        Public Overloads Sub DrawCircle(ByVal X As Integer, ByVal Y As Integer, ByVal Radius As Integer, Optional ByVal xWidth As Integer = 1)
            _G.DrawArc(New Pen(Color, xWidth), New Rectangle(X - Radius, Y - Radius, Radius * 2, Radius * 2), 0, 360)
        End Sub

        ''' <summary>
        ''' Draw a circle with a center point and a radius, but with start and end angles
        ''' </summary>
        ''' <param name="X"></param>
        ''' <param name="Y"></param>
        ''' <param name="Radius"></param>
        ''' <param name="cColor"></param>
        ''' <param name="StartAngle"></param>
        ''' <param name="SweepAngle"></param>
        Public Overloads Shared Sub DrawCircle(X As Long, Y As Long, Radius As Long, cColor As Pen, Optional StartAngle As Integer = 0, Optional SweepAngle As Integer = 360)
            _G.DrawArc(cColor, CLng(X - (Radius / 2)), CLng(Y - (Radius / 2)), Radius, Radius, StartAngle, SweepAngle)
        End Sub

        ''' <summary>
        ''' Draw a closed curve with multiple points
        ''' </summary>
        ''' <param name="Points"></param>
        ''' <param name="Tension"></param>
        ''' <param name="FillMode"></param>
        ''' <param name="xWidth"></param>
        Public Sub DrawClosedCurve(ByVal Points As PointF(), ByVal Tension As Single, Optional ByVal FillMode As FillMode = FillMode.Alternate, Optional ByVal xWidth As Integer = 1)
            _G.DrawClosedCurve(New Pen(Color, xWidth), Points, Tension, FillMode)
        End Sub

        ''' <summary>
        ''' Draw curve with multiple points
        ''' </summary>
        ''' <param name="Points"></param>
        ''' <param name="offSet"></param>
        ''' <param name="NumberOfSegments"></param>
        ''' <param name="Tension"></param>
        ''' <param name="xWidth"></param>
        Public Sub DrawCurve(ByVal Points As Point(), ByVal offSet As Integer, ByVal NumberOfSegments As Integer, ByVal Tension As Single, Optional xWidth As Integer = 1)
            _G.DrawCurve(New Pen(Color, xWidth), Points, offSet, NumberOfSegments, Tension)
        End Sub

        ''' <summary>
        ''' Draw an Ellipse
        ''' </summary>
        ''' <param name="X"></param>
        ''' <param name="Y"></param>
        ''' <param name="Width"></param>
        ''' <param name="Height"></param>
        ''' <param name="xWidth"></param>
        Public Sub DrawEllipse(ByVal X As Integer, ByVal Y As Integer, ByVal Width As Integer, ByVal Height As Integer, Optional xWidth As Integer = 1)
            _G.DrawEllipse(New Pen(Color, xWidth), X, Y, Width, Height)
        End Sub

        ''' <summary>
        ''' Draw an image with all the information you can give
        ''' </summary>
        ''' <param name="Image"></param>
        ''' <param name="dX"></param>
        ''' <param name="dY"></param>
        ''' <param name="dWidth"></param>
        ''' <param name="dHeight"></param>
        ''' <param name="srcX"></param>
        ''' <param name="srcY"></param>
        ''' <param name="srcWidth"></param>
        ''' <param name="srcHeight"></param>
        ''' <param name="srcUnit"></param>
        Public Overloads Shared Sub DrawImage(ByVal Image As Image, ByVal dX As Integer, ByVal dY As Integer, ByVal dWidth As Integer, ByVal dHeight As Integer,
                                           ByVal srcX As Integer, ByVal srcY As Integer, ByVal srcWidth As Integer, ByVal srcHeight As Integer,
                                           Optional ByVal srcUnit As GraphicsUnit = GraphicsUnit.Pixel)
            _G.DrawImage(Image, New Rectangle(dX, dY, dWidth, dHeight), srcX, srcY, srcWidth, srcHeight, srcUnit)
        End Sub

        ''' <summary>
        ''' Draw an image just with a source file and location
        ''' </summary>
        ''' <param name="SourceFile"></param>
        ''' <param name="X"></param>
        ''' <param name="Y"></param>
        Public Overloads Shared Sub DrawImage(SourceFile As String, X As Long, Y As Long)
            _G.DrawImage(New Bitmap(SourceFile), X, Y)
        End Sub

        ''' <summary>
        ''' Draw a line with 4 coordinates
        ''' </summary>
        ''' <param name="X"></param>
        ''' <param name="Y"></param>
        ''' <param name="X1"></param>
        ''' <param name="Y1"></param>
        ''' <param name="xWidth"></param>
        Public Sub DrawLine(ByVal X As Integer, ByVal Y As Integer, ByVal X1 As Integer, ByVal Y1 As Integer, Optional ByVal xWidth As Integer = 1)
            _G.DrawLine(New Pen(Color, xWidth), New PointF(X, Y), New PointF(X1, Y1))
        End Sub

        ''' <summary>
        ''' Draw a path
        ''' </summary>
        ''' <param name="Path"></param>
        ''' <param name="xWidth"></param>
        Public Sub DrawPath(ByVal Path As GraphicsPath, Optional ByVal xWidth As Integer = 1)
            _G.DrawPath(New Pen(Color, xWidth), Path)
        End Sub

        ''' <summary>
        ''' Draw a Pie
        ''' </summary>
        ''' <param name="X1"></param>
        ''' <param name="Y1"></param>
        ''' <param name="Width"></param>
        ''' <param name="Height"></param>
        ''' <param name="SweepAngle"></param>
        ''' <param name="StartAngle"></param>
        Public Sub DrawPie(ByVal X1 As Integer, ByVal Y1 As Integer, ByVal Width As Integer, ByVal Height As Integer, ByVal SweepAngle As Integer, Optional ByVal StartAngle As Integer = 0, Optional ByVal xWidth As Integer = 1)
            _G.DrawPie(New Pen(Color, xWidth), New RectangleF(X1, Y1, Width, Height), StartAngle, SweepAngle)
        End Sub

        ''' <summary>
        ''' Draw a polygon
        ''' </summary>
        ''' <param name="Points"></param>
        ''' <param name="xWidth"></param>
        Public Sub DrawPolygon(ByVal Points As PointF(), Optional ByVal xWidth As Integer = 1)
            _G.DrawPolygon(New Pen(Color, xWidth), Points)
        End Sub

        ''' <summary>
        ''' Draw a rectangle with location, height and width
        ''' </summary>
        ''' <param name="X"></param>
        ''' <param name="Y"></param>
        ''' <param name="Width"></param>
        ''' <param name="Height"></param>
        ''' <param name="xWidth"></param>
        Public Overloads Sub DrawRectangle(ByVal X As Integer, ByVal Y As Integer, ByVal Width As Integer, ByVal Height As Integer, Optional ByVal xWidth As Integer = 1)
            _G.DrawRectangle(New Pen(Color, xWidth), New Rectangle(X, Y, Width, Height))
        End Sub

        ''' <summary>
        ''' Draw a rectangle with the coordinates of 2 points
        ''' </summary>
        ''' <param name="X"></param>
        ''' <param name="Y"></param>
        ''' <param name="X1"></param>
        ''' <param name="Y1"></param>
        ''' <param name="cColor"></param>
        Public Overloads Shared Sub DrawRectangle(X As Long, Y As Long, X1 As Long, Y1 As Long, cColor As Pen)
            _G.DrawRectangle(cColor, X, Y, X1 - X, Y1 - Y)
        End Sub

        ''' <summary>
        ''' Draw a square, is just like a rectangle, but only with width
        ''' </summary>
        ''' <param name="X"></param>
        ''' <param name="Y"></param>
        ''' <param name="Width"></param>
        ''' <param name="PenWidth"></param>
        Public Sub DrawSquare(ByVal X As Integer, ByVal Y As Integer, ByVal Width As Integer, Optional ByVal PenWidth As Integer = 1)
            _G.DrawRectangle(New Pen(Color, PenWidth), New Rectangle(X, Y, Width, Width))
        End Sub

        ''' <summary>
        ''' Draw a string
        ''' </summary>
        ''' <param name="X"></param>
        ''' <param name="Y"></param>
        ''' <param name="st"></param>
        ''' <param name="Fnt"></param>
        ''' <param name="xFormat"></param>
        Public Sub DrawString(ByVal X As Integer, ByVal Y As Integer, ByVal st As String, ByVal Fnt As Font, Optional ByVal xFormat As StringFormat = Nothing)
            _G.DrawString(st, Fnt, New SolidBrush(Color), New PointF(X, Y), xFormat)
        End Sub

        ''' <summary>
        ''' Draw a Rounded rectangle
        ''' </summary>
        ''' <param name="X"></param>
        ''' <param name="Y"></param>
        ''' <param name="X1"></param>
        ''' <param name="Y1"></param>
        ''' <param name="Radius"></param>
        ''' <param name="xWidth"></param>
        Public Overloads Shared Sub DrawRoundRectangle(X As Long, Y As Long, X1 As Long, Y1 As Long, Radius As Long, Optional ByVal xWidth As Integer = 1)
            With _G
                .DrawArc(New Pen(Color, xWidth), New Rectangle(CInt(X), CInt(Y), CInt(Radius), CInt(Radius)), 180, 90)
                .DrawArc(New Pen(Color, xWidth), New Rectangle(CInt(X1 - Radius), CInt(Y), CInt(Radius), CInt(Radius)), 270, 90)
                .DrawArc(New Pen(Color, xWidth), X, Y1 - Radius, Radius, Radius, 90, 90)
                .DrawArc(New Pen(Color, xWidth), X1 - Radius, Y1 - Radius, Radius, Radius, 0, 90)

                .DrawLine(New Pen(Color, xWidth), X, CLng(Y + (Radius / 2)), X, CLng(Y1 - (Radius / 2)))
                .DrawLine(New Pen(Color, xWidth), CLng(X + (Radius / 2)), Y, CLng(X1 - (Radius / 2)), Y)
                .DrawLine(New Pen(Color, xWidth), X1, CLng(Y + (Radius / 2)), X1, CLng(Y1 - (Radius / 2)))
                .DrawLine(New Pen(Color, xWidth), CLng(X + (Radius / 2)), Y1, CLng(X1 - (Radius / 2)), Y1)
            End With
        End Sub

        ''' <summary>
        ''' Draw a rounded rectangle
        ''' </summary>
        ''' <param name="X"></param>
        ''' <param name="Y"></param>
        ''' <param name="X1"></param>
        ''' <param name="Y1"></param>
        ''' <param name="Radius"></param>
        ''' <param name="cColor"></param>
        Public Overloads Shared Sub DrawRoundRectangle(X As Long, Y As Long, X1 As Long, Y1 As Long, Radius As Long, cColor As Pen)
            With _G
                .DrawArc(cColor, New Rectangle(CInt(X), CInt(Y), CInt(Radius), CInt(Radius)), 180, 90)
                .DrawArc(cColor, New Rectangle(CInt(X1 - Radius), CInt(Y), CInt(Radius), CInt(Radius)), 270, 90)
                .DrawArc(cColor, X, Y1 - Radius, Radius, Radius, 90, 90)
                .DrawArc(cColor, X1 - Radius, Y1 - Radius, Radius, Radius, 0, 90)

                .DrawLine(cColor, X, CLng(Y + (Radius / 2)), X, CLng(Y1 - (Radius / 2)))
                .DrawLine(cColor, CLng(X + (Radius / 2)), Y, CLng(X1 - (Radius / 2)), Y)
                .DrawLine(cColor, X1, CLng(Y + (Radius / 2)), X1, CLng(Y1 - (Radius / 2)))
                .DrawLine(cColor, CLng(X + (Radius / 2)), Y1, CLng(X1 - (Radius / 2)), Y1)
            End With
        End Sub

        ''' <summary>
        ''' Draw an hexagon
        ''' </summary>
        ''' <param name="X"></param>
        ''' <param name="Y"></param>
        ''' <param name="X1"></param>
        ''' <param name="Y1"></param>
        ''' <param name="penWidth"></param>
        Public Overloads Shared Sub DrawHexagon(X As Long, Y As Long, X1 As Long, Y1 As Long, Optional penWidth As Integer = 1)
            Dim Radius As Long = Tools.RadiusFromTriangle(X, Y, X1, Y1)
            Dim CenterX As Long = CLng((Abs(X - X1) / 2) + X)
            Dim CenterY As Long = CLng((Abs(Y - Y1) / 2) + Y)

            With _G
                .DrawLine(New Pen(Color, penWidth), X, Y, X1, Y)
                .DrawLine(New Pen(Color, penWidth), X, Y1, X1, Y1)

                .DrawLine(New Pen(Color, penWidth), X, Y, CenterX - Radius, CenterY)
                .DrawLine(New Pen(Color, penWidth), CenterX - Radius, CenterY, X, Y1)

                .DrawLine(New Pen(Color, penWidth), X1, Y, CenterX + Radius, CenterY)
                .DrawLine(New Pen(Color, penWidth), X1, Y1, CenterX + Radius, CenterY)
            End With
        End Sub

        ''' <summary>
        ''' Draw an Hexagon
        ''' </summary>
        ''' <param name="X"></param>
        ''' <param name="Y"></param>
        ''' <param name="X1"></param>
        ''' <param name="Y1"></param>
        ''' <param name="cColor"></param>
        Public Overloads Shared Sub DrawHexagon(X As Long, Y As Long, X1 As Long, Y1 As Long, cColor As Pen)
            Dim Radius As Long = Tools.RadiusFromTriangle(X, Y, X1, Y1)
            Dim CenterX As Long = CLng((Abs(X - X1) / 2) + X)
            Dim CenterY As Long = CLng((Abs(Y - Y1) / 2) + Y)

            With _G
                .DrawLine(cColor, X, Y, X1, Y)
                .DrawLine(cColor, X, Y1, X1, Y1)

                .DrawLine(cColor, X, Y, CenterX - Radius, CenterY)
                .DrawLine(cColor, CenterX - Radius, CenterY, X, Y1)

                .DrawLine(cColor, X1, Y, CenterX + Radius, CenterY)
                .DrawLine(cColor, X1, Y1, CenterX + Radius, CenterY)
            End With
        End Sub
#Region "iDisposable"
        Private disposedValue As Boolean

        Protected Overridable Sub Dispose(disposing As Boolean)
            If Not disposedValue Then
                If disposing Then : End If
                disposedValue = True
            End If
        End Sub

        Public Sub Dispose() Implements IDisposable.Dispose
            Dispose(disposing:=True)
            GC.SuppressFinalize(Me)
        End Sub
#End Region
    End Class

    Public Class DrawText

        Shared Color As Color
        Private Shared _G As Graphics

        Private sbBlue As SolidBrush() = {New SolidBrush(Color.FromArgb(200, 0, 0, 255)),
                                     New SolidBrush(Color.FromArgb(200, 0, 40, 255)),
                                     New SolidBrush(Color.FromArgb(200, 0, 80, 255)),
                                     New SolidBrush(Color.FromArgb(200, 0, 120, 255)),
                                     New SolidBrush(Color.FromArgb(200, 0, 160, 255))}

        Private penBlue As Pen() = {New Pen(Color.FromArgb(200, 0, 0, 255)),
                               New Pen(Color.FromArgb(200, 0, 40, 255)),
                               New Pen(Color.FromArgb(200, 0, 80, 255)),
                               New Pen(Color.FromArgb(200, 0, 120, 255)),
                               New Pen(Color.FromArgb(200, 0, 160, 255))}

        ''' <summary>
        ''' Initialize graphics
        ''' </summary>
        ''' <param name="myGraphics"></param>
        ''' <param name="mColor"></param>
        ''' <param name="ComposeQuality"></param>
        ''' <param name="SmoothMode"></param>
        Public Sub New(myGraphics As Graphics,
                   Optional mColor As Color = Nothing,
                   Optional ComposeQuality As CompositingQuality = CompositingQuality.HighQuality,
                   Optional SmoothMode As SmoothingMode = SmoothingMode.AntiAlias)

            _G = myGraphics
            _G.CompositingQuality = ComposeQuality
            _G.SmoothingMode = SmoothMode

            If mColor = Nothing Then
                Color = Color.Black
            Else
                Color = mColor
            End If
        End Sub

        ''' <summary>
        ''' Draw tect with Emboss effect
        ''' </summary>
        ''' <param name="Text"></param>
        ''' <param name="X"></param>
        ''' <param name="Y"></param>
        ''' <param name="Color"></param>
        ''' <param name="Depth"></param>
        ''' <param name="FontName"></param>
        ''' <param name="FontSize"></param>
        ''' <param name="FontBold"></param>
        Public Sub DrawTextWithEmboss(Text As String, X As Long, Y As Long, Color As Color, Optional Depth As Long = 10, Optional FontName As String = "Arial", Optional FontSize As Long = 30, Optional FontBold As Boolean = False)
            Dim textSize As SizeF

            Dim objForeBrush As Brush = Brushes.White
            Dim objFont As New Font(FontName, FontSize, CType(IIf(Not FontBold, FontStyle.Regular, FontStyle.Bold), FontStyle))

            textSize = _G.MeasureString(Text, objFont)
            _G.DrawString(Text, objFont, Brushes.Gray,
                X + Depth,
                Y + Depth)
            _G.DrawString(Text, objFont, objForeBrush, X, Y)
        End Sub

        ''' <summary>
        ''' Draw text with shear effect
        ''' </summary>
        ''' <param name="Text"></param>
        ''' <param name="X"></param>
        ''' <param name="Y"></param>
        ''' <param name="Color"></param>
        ''' <param name="Shear"></param>
        ''' <param name="FontName"></param>
        ''' <param name="FontSize"></param>
        ''' <param name="FontBold"></param>
        Public Sub DrawTextWithShear(Text As String, X As Long, Y As Long, Color As Color, Optional Shear As Single = -0.5, Optional FontName As String = "Arial", Optional FontSize As Long = 30, Optional FontBold As Boolean = False)
            Dim textSize As SizeF
            Dim TextForeBrush As Brush = New SolidBrush(Color)
            Dim TextFont As New Font(FontName, FontSize, CType(IIf(Not FontBold, FontStyle.Regular, FontStyle.Bold), FontStyle))
            Dim TextTransform As Matrix

            'find input text size
            textSize = _G.MeasureString(Text, TextFont)

            _G.TranslateTransform(X, Y)

            TextTransform = _G.Transform
            TextTransform.Shear(Shear, 0)
            _G.Transform = TextTransform

            _G.DrawString(Text, TextFont, TextForeBrush, 0, 0)
        End Sub

        ''' <summary>
        ''' Draw text with multiple lines
        ''' </summary>
        ''' <param name="Text"></param>
        ''' <param name="X"></param>
        ''' <param name="Y"></param>
        ''' <param name="X1"></param>
        ''' <param name="Y1"></param>
        ''' <param name="Color"></param>
        ''' <param name="FontName"></param>
        ''' <param name="FontSize"></param>
        ''' <param name="FontBold"></param>
        Public Sub DrawMultiLine(Text As String, X As Long, Y As Long, X1 As Long, Y1 As Long, Color As Color, Optional FontName As String = "Arial", Optional FontSize As Long = 30, Optional FontBold As Boolean = False)
            Dim textSize As SizeF
            Dim myForeBrush As Brush = New SolidBrush(Color)
            Dim myFont As Font = New Font(FontName, FontSize, CType(IIf(Not FontBold, FontStyle.Regular, FontStyle.Bold), FontStyle))

            ' Find the Size required to draw the Sample Text.
            textSize = _G.MeasureString(Text, myFont)

            ' Draw the main text.
            _G.DrawString(Text, myFont, myForeBrush, New RectangleF(0, 0, X1 - X, Y1 - Y))
        End Sub

        ''' <summary>
        ''' Draw text with gradient effect
        ''' </summary>
        ''' <param name="Text"></param>
        ''' <param name="X"></param>
        ''' <param name="Y"></param>
        ''' <param name="X1"></param>
        ''' <param name="Y1"></param>
        ''' <param name="Color1"></param>
        ''' <param name="Color2"></param>
        ''' <param name="FontName"></param>
        ''' <param name="FontSize"></param>
        ''' <param name="FontBold"></param>
        Public Sub DrawGradientText(Text As String, X As Long, Y As Long, X1 As Long, Y1 As Long, Color1 As Color, Color2 As Color, Optional FontName As String = "Arial", Optional FontSize As Long = 30, Optional FontBold As Boolean = False)
            Dim textSize As SizeF
            Dim myBrush As Brush
            Dim gradientRectangle As RectangleF
            Dim myFont As Font = New Font(FontName, FontSize, CType(IIf(Not FontBold, FontStyle.Regular, FontStyle.Bold), FontStyle))

            ' Find the Size required to draw the Sample Text.
            textSize = _G.MeasureString(Text, myFont)

            ' Create a Diagonal Gradient LinearGradientBrush.
            gradientRectangle = New RectangleF(New PointF(0, 0), textSize)
            myBrush = New LinearGradientBrush(gradientRectangle, Color1, Color2, LinearGradientMode.ForwardDiagonal)

            ' Draw the text.
            _G.DrawString(Text, myFont, myBrush, X, Y)
        End Sub

        ''' <summary>
        ''' Draw text with a shadow effect
        ''' </summary>
        ''' <param name="Text"></param>
        ''' <param name="X"></param>
        ''' <param name="Y"></param>
        ''' <param name="X1"></param>
        ''' <param name="Y1"></param>
        ''' <param name="ShadowDepth"></param>
        ''' <param name="ForeColor"></param>
        ''' <param name="ShadowColor"></param>
        ''' <param name="FontName"></param>
        ''' <param name="FontSize"></param>
        ''' <param name="FontBold"></param>
        Public Sub DrawShadowText(Text As String, X As Long, Y As Long, X1 As Long, Y1 As Long, ShadowDepth As Long, ForeColor As Color, ShadowColor As Color, Optional FontName As String = "Arial", Optional FontSize As Long = 30, Optional FontBold As Boolean = False)
            Dim textSize As SizeF
            Dim myForeBrush As New SolidBrush(ForeColor)
            Dim myShadowBrush As New SolidBrush(ShadowColor)

            Dim fontconverter As New FontConverter
            Dim myFont As Font = New Font(FontName, FontSize, CType(IIf(Not FontBold, FontStyle.Regular, FontStyle.Bold), FontStyle))

            textSize = _G.MeasureString(Text, myFont)

            ' Draw the Shadow first.
            _G.DrawString(Text, myFont, myShadowBrush, X + ShadowDepth, Y + ShadowDepth)

            ' Draw the Main text over the shadow.
            _G.DrawString(Text, myFont, myForeBrush, X, Y)
        End Sub

        ''' <summary>
        ''' Draw text with outlined effect
        ''' </summary>
        ''' <param name="H"></param>
        ''' <param name="V"></param>
        ''' <param name="X"></param>
        ''' <param name="Y"></param>
        ''' <param name="Width"></param>
        ''' <param name="Height"></param>
        ''' <param name="TextFont"></param>
        ''' <param name="TextColor"></param>
        ''' <param name="ContourColor"></param>
        ''' <param name="ContourWidth"></param>
        Public Sub DrawOulinedText(H As String, V As String, X As Long, Y As Long, Width As Long, Height As Long, TextFont As Font, TextColor As Color, ContourColor As Color, Optional ContourWidth As Integer = 1)
            Dim outlinePath As GraphicsPath
            Dim Size As SizeF

            Dim stringFormat As New StringFormat()
            stringFormat.Alignment = StringAlignment.Center
            stringFormat.LineAlignment = StringAlignment.Center

            With _G
                .TextRenderingHint = Text.TextRenderingHint.AntiAlias
                .SmoothingMode = SmoothingMode.AntiAlias

                outlinePath = New GraphicsPath
                Size = .MeasureString(H, TextFont)
                outlinePath.AddString(H, TextFont.FontFamily,
                                     TextFont.Style,
                                     TextFont.Size,
                                     New Point(0, 0),
                                     stringFormat)

                .TranslateTransform(CSng((Width / 2) + X), CSng(Y + (Height / 2)))
                .FillPath(New SolidBrush(TextColor), outlinePath)
                .DrawPath(New Pen(ContourColor, ContourWidth), outlinePath)
                .ResetTransform()

                outlinePath.Dispose()

                outlinePath = New GraphicsPath
                Size = .MeasureString(V, TextFont)
                outlinePath.AddString(V, TextFont.FontFamily,
                                     TextFont.Style,
                                     TextFont.Size / 2,
                                     New Point(0, 0),
                                     stringFormat)

                .TranslateTransform(CSng((Width - ((Width / 5) / 2)) + X), CSng(Y + (Height / 2)))
                .RotateTransform(-90)
                .FillPath(New SolidBrush(TextColor), outlinePath)
                .DrawPath(New Pen(ContourColor, ContourWidth), outlinePath)
                .ResetTransform()

                outlinePath.Dispose()
            End With

        End Sub
    End Class

    Public Class SG_DrawTools : Implements IDisposable
        Private Shared _G As Graphics

        Private sbBlue As SolidBrush() = {New SolidBrush(Color.FromArgb(200, 0, 0, 255)),
                                     New SolidBrush(Color.FromArgb(200, 0, 40, 255)),
                                     New SolidBrush(Color.FromArgb(200, 0, 80, 255)),
                                     New SolidBrush(Color.FromArgb(200, 0, 120, 255)),
                                     New SolidBrush(Color.FromArgb(200, 0, 160, 255))}

        Private penBlue As Pen() = {New Pen(Color.FromArgb(200, 0, 0, 255)),
                               New Pen(Color.FromArgb(200, 0, 40, 255)),
                               New Pen(Color.FromArgb(200, 0, 80, 255)),
                               New Pen(Color.FromArgb(200, 0, 120, 255)),
                               New Pen(Color.FromArgb(200, 0, 160, 255))}

        Public ReadOnly DPI As Structures.DPI_INfo
        Public Color As Color

        ''' <summary>
        ''' Initilize the class
        ''' </summary>
        ''' <param name="myGraphics"></param>
        ''' <param name="mColor"></param>
        ''' <param name="ComposeQuality"></param>
        ''' <param name="SmoothMode"></param>
        Public Sub New(myGraphics As Graphics,
                   Optional mColor As Color = Nothing,
                   Optional ComposeQuality As CompositingQuality = CompositingQuality.HighQuality,
                   Optional SmoothMode As SmoothingMode = SmoothingMode.AntiAlias)

            _G = myGraphics
            _G.CompositingQuality = ComposeQuality
            _G.SmoothingMode = SmoothMode

            If mColor = Nothing Then
                Color = Color.Black
            Else
                Color = mColor
            End If
        End Sub

        ''' <summary>
        ''' Convert the type of an image
        ''' </summary>
        ''' <param name="SourceFile"></param>
        ''' <param name="DestinationFile"></param>
        ''' <param name="DestinationType"></param>
        ''' <returns></returns>
        Public Function ConvertImageType(SourceFile As String, DestinationFile As String, DestinationType As Enums.DestinationTypes) As Boolean
            Dim imgFormat As Imaging.ImageFormat
            Dim Dest As String = ""

            Select Case DestinationType
                Case Enums.DestinationTypes.BITMAP
                    imgFormat = Imaging.ImageFormat.Bmp
                    Dest = DestinationFile & ".bmp"
                Case Enums.DestinationTypes.JPEG
                    imgFormat = Imaging.ImageFormat.Jpeg
                    Dest = DestinationFile & ".jpeg"
                Case Enums.DestinationTypes.PNG
                    imgFormat = Imaging.ImageFormat.Png
                    Dest = DestinationFile & ".png"
                Case Enums.DestinationTypes.GIF
                    imgFormat = Imaging.ImageFormat.Gif
                    Dest = DestinationFile & ".gif"
                Case Else
                    imgFormat = Imaging.ImageFormat.Icon
                    Dest = DestinationFile & ".icon"
            End Select

            Try
                Dim SourceImg As Image = Image.FromFile(SourceFile)
                SourceImg.Save(Dest, imgFormat)

            Catch ex As Exception
                Return False
            End Try

            Return True
        End Function

        ''' <summary>
        ''' Draw a simple rectangle
        ''' </summary>
        ''' <param name="X"></param>
        ''' <param name="Y"></param>
        ''' <param name="X1"></param>
        ''' <param name="Y1"></param>
        Public Sub RectangleFrame(X As Long, Y As Long, X1 As Long, Y1 As Long)
            For i As Integer = 0 To 4
                Draw.DrawRectangle(X + i, Y + i, X1 - i, Y1 - i, penBlue(i))
            Next
        End Sub

        ''' <summary>
        ''' Draw a round Frame with 4 RoundRectagles
        ''' </summary>
        ''' <param name="X"></param>
        ''' <param name="Y"></param>
        ''' <param name="X1"></param>
        ''' <param name="Y1"></param>
        ''' <param name="Radius"></param>
        Public Sub RoundFrame(X As Long, Y As Long, X1 As Long, Y1 As Long, Radius As Long)
            For i As Integer = 0 To 4
                Draw.DrawRoundRectangle(X + i, Y + i, X1 - i, Y1 - i, Radius, penBlue(i))
            Next
        End Sub

        ''' <summary>
        ''' Draw a circular Frame with 4 circles
        ''' </summary>
        ''' <param name="X"></param>
        ''' <param name="Y"></param>
        ''' <param name="Radius"></param>
        Public Sub CircleFrame(X As Long, Y As Long, Radius As Long)
            For i As Integer = 0 To 4
                Draw.DrawCircle(X, Y, Radius - i, penBlue(i))
            Next
        End Sub

        ''' <summary>
        ''' Draw an Hexagonic fram with 4 hexagons
        ''' </summary>
        ''' <param name="X"></param>
        ''' <param name="Y"></param>
        ''' <param name="X1"></param>
        ''' <param name="Y1"></param>
        Public Sub HexagonFrame(X As Long, Y As Long, X1 As Long, Y1 As Long)
            For i As Integer = 0 To 4
                Draw.DrawHexagon(X + i, Y + i, X1 - i, Y1 - i, penBlue(i))
            Next
        End Sub

        ''' <summary>
        ''' Draw horizontal Lines
        ''' </summary>
        ''' <param name="X"></param>
        ''' <param name="Y"></param>
        ''' <param name="Y1"></param>
        Public Sub DrawVBlueLine(X As Long, Y As Long, Y1 As Long)
            For i As Integer = 0 To 4
                _G.DrawLine(penBlue(i), X + i, Y, X + i, Y1)
            Next

            For i As Integer = 3 To 0 Step -1
                _G.DrawLine(penBlue(i), (X + 5) - i, Y, (X + 5) - i, Y1)
            Next
        End Sub

        ''' <summary>
        ''' Draw Vertical Lines
        ''' </summary>
        ''' <param name="X"></param>
        ''' <param name="Y"></param>
        ''' <param name="X1"></param>
        Public Sub DrawHBlueLine(X As Long, Y As Long, X1 As Long)
            For i As Integer = 0 To 4
                _G.DrawLine(penBlue(i), X, Y + i, X1, Y + i)
            Next

            For i As Integer = 3 To 0 Step -1
                _G.DrawLine(penBlue(i), X, (Y + 5) - i, X1, (Y + 5) - i)
            Next
        End Sub

        ''' <summary>
        ''' Make a grid of squares
        ''' </summary>
        ''' <param name="X"></param>
        ''' <param name="Y"></param>
        ''' <param name="X1"></param>
        ''' <param name="Y1"></param>
        ''' <param name="xWidth"></param>
        ''' <param name="GridStep"></param>
        Public Sub SquareGrid(X As Long, Y As Long, X1 As Long, Y1 As Long, Optional ByVal xWidth As Integer = 1, Optional GridStep As Long = 10)
            With _G
                .DrawRectangle(New Pen(Color, xWidth), X, Y, X1 - X, Y1 - Y)

                For XX As Long = X To X1 Step GridStep
                    .DrawLine(New Pen(Color, xWidth), XX, Y, XX, Y1)
                Next

                For YY As Long = Y To Y1 Step GridStep
                    .DrawLine(New Pen(Color, xWidth), X, YY, X1, YY)
                Next
            End With
        End Sub

        ''' <summary>
        ''' Make a grid of blue squares
        ''' </summary>
        ''' <param name="X"></param>
        ''' <param name="Y"></param>
        ''' <param name="X1"></param>
        ''' <param name="Y1"></param>
        ''' <param name="GridStep"></param>
        Public Sub SquareGridBlue(X As Long, Y As Long, X1 As Long, Y1 As Long, Optional GridStep As Long = 15)
            With _G
                For i As Integer = 0 To 4
                    .DrawRectangle(penBlue(i), X + i, Y + i, (X1 - X) - i, (Y1 - Y) - i)
                Next

                For XX As Long = X To X1 Step GridStep
                    DrawVBlueLine(XX, Y, Y1)
                Next

                For YY As Long = Y To Y1 Step GridStep
                    DrawHBlueLine(X, YY, X1)
                Next
            End With
        End Sub

        ''' <summary>
        ''' This function returns a gradient with a selected color, uses Enum Back
        ''' </summary>
        ''' <param name="BaseRect"></param>
        ''' <param name="BGC"></param>
        ''' <returns></returns>
        Public Function GenerateGradient(BaseRect As Rectangle, BGC As Enums.BackGroundColor) As LinearGradientBrush
            Select Case BGC
                Case Enums.BackGroundColor.Blue
                    Return New LinearGradientBrush(BaseRect, Color.AliceBlue, Color.Blue, LinearGradientMode.ForwardDiagonal)
                Case Enums.BackGroundColor.Green
                    Return New LinearGradientBrush(BaseRect, Color.Green, Color.GreenYellow, LinearGradientMode.ForwardDiagonal)
                Case Enums.BackGroundColor.Gray
                    Return New LinearGradientBrush(BaseRect, Color.DimGray, Color.Gray, LinearGradientMode.ForwardDiagonal)
                Case Enums.BackGroundColor.Orange
                    Return New LinearGradientBrush(BaseRect, Color.Orange, Color.DarkOrange, LinearGradientMode.ForwardDiagonal)
                Case Enums.BackGroundColor.Pink
                    Return New LinearGradientBrush(BaseRect, Color.LightPink, Color.HotPink, LinearGradientMode.ForwardDiagonal)
                Case Enums.BackGroundColor.Red
                    Return New LinearGradientBrush(BaseRect, Color.Red, Color.DarkRed, LinearGradientMode.ForwardDiagonal)
                Case Enums.BackGroundColor.Yellow
                    Return New LinearGradientBrush(BaseRect, Color.Yellow, Color.LightGoldenrodYellow, LinearGradientMode.ForwardDiagonal)
                Case Else
                    Return New LinearGradientBrush(BaseRect, Color.WhiteSmoke, Color.White, LinearGradientMode.ForwardDiagonal)
            End Select
        End Function

        ''' <summary>
        ''' This function Makes a color lighter. You can Use a internal function to do this, that makes a color lighter, but do not allow a simple use
        ''' </summary>
        ''' <param name="Color"></param>
        ''' <param name="CorrectionFactor"></param>
        ''' <returns></returns>
        Public Function ColorLight(Color As Color, CorrectionFactor As Single) As Color
            Dim red As Single = (255 - Color.R) * CorrectionFactor + Color.R
            Dim green As Single = (255 - Color.G) * CorrectionFactor + Color.G
            Dim blue As Single = (255 - Color.B) * CorrectionFactor + Color.B
            Dim ColorLightChanged As Color = Color.FromArgb(Color.A, CInt(red), CInt(green), CInt(blue))

            Return ColorLightChanged
        End Function

        ''' <summary>
        ''' Converts the value of and RGB Color to a system color
        ''' </summary>
        ''' <param name="r"></param>
        ''' <param name="g"></param>
        ''' <param name="b"></param>
        ''' <returns></returns>
        Public Function RGBColor(ByVal r As Integer, ByVal g As Integer, ByVal b As Integer) As Color
            If r > 255 Then r = 255
            If r < 0 Then r = 0
            If g > 255 Then g = 255
            If g < 0 Then g = 0
            If b > 255 Then b = 255
            If b < 0 Then b = 0

            Return ColorTranslator.FromWin32(RGB(r, g, b))
        End Function

        ''' <summary>
        ''' Converts a HSB Color to a System Color, uses RGBColor and HueColor
        ''' </summary>
        ''' <param name="hue"></param>
        ''' <param name="saturation"></param>
        ''' <param name="brightness"></param>
        ''' <returns></returns>
        Public Function HSBColor(ByVal hue As Single, ByVal saturation As Single, ByVal brightness As Single) As Color
            If hue < 0 Then hue = 0
            If hue > 359 Then hue = hue - 360
            If saturation < 0 Then saturation = 0
            If saturation > 1 Then saturation = 1
            If brightness < 0 Then brightness = 0
            If brightness > 1 Then brightness = 1

            Dim v1, v2, vh As Single
            Dim r, g, b As Integer

            If saturation = 0 Then
                Return RGBColor(CInt(brightness * 255), CInt(brightness * 255), CInt(brightness * 255))
            Else
                If brightness < 0.5 Then
                    v2 = brightness * (1 + saturation)
                Else
                    v2 = (brightness + saturation) - (brightness * saturation)
                End If

                v1 = 2 * brightness - v2
                vh = hue / 360

                r = CInt(255 * HueColor(v1, v2, CSng(vh + (1 / 3))))
                g = CInt(255 * HueColor(v1, v2, vh))
                b = CInt(255 * HueColor(v1, v2, CSng(vh - (1 / 3))))

                Return RGBColor(r, g, b)
            End If
        End Function

        ''' <summary>
        ''' Convert Hue value color to numeric color
        ''' </summary>
        ''' <param name="v1"></param>
        ''' <param name="v2"></param>
        ''' <param name="vH"></param>
        ''' <returns></returns>
        Private Function HueColor(ByVal v1 As Single, ByVal v2 As Single, ByVal vH As Single) As Single
            If (vH < 0) Then vH += 1
            If (vH > 1) Then vH -= 1

            If ((6 * vH) < 1) Then
                Return (v1 + (v2 - v1) * 6 * vH)
            ElseIf ((2 * vH) < 1) Then
                Return (v2)
            ElseIf ((3 * vH) < 2) Then
                Return CSng((v1 + (v2 - v1) * ((2 / 3) - vH) * 6))
            Else
                Return (v1)
            End If
        End Function

        ''' <summary>
        ''' Mix two colors and based on a Opacity level, uses RGBColor
        ''' </summary>
        ''' <param name="BlendColor"></param>
        ''' <param name="BaseColor"></param>
        ''' <param name="Opacity"></param>
        ''' <returns></returns>
        Public Function OpacityMix(ByVal BlendColor As Color, ByVal BaseColor As Color, ByVal Opacity As Integer) As Color
            Dim r1, g1, b1 As Integer
            Dim r2, g2, b2 As Integer
            Dim r3, g3, b3 As Integer

            r1 = BlendColor.R
            g1 = BlendColor.G
            b1 = BlendColor.B

            r2 = BaseColor.R
            g2 = BaseColor.G
            b2 = BaseColor.B

            r3 = CInt(((r1 * (Opacity / 100)) + (r2 * (1 - (Opacity / 100)))))
            g3 = CInt(((g1 * (Opacity / 100)) + (g2 * (1 - (Opacity / 100)))))
            b3 = CInt(((b1 * (Opacity / 100)) + (b2 * (1 - (Opacity / 100)))))

            Return RGBColor(r3, g3, b3)
        End Function

        ''' <summary>
        ''' Invert a color, uses RGBColor
        ''' </summary>
        ''' <param name="c"></param>
        ''' <returns></returns>
        Public Function InvertColor(ByVal c As Color) As Color
            Dim r1, g1, b1 As Integer
            Dim r2, g2, b2 As Integer

            r1 = c.R
            g1 = c.G
            b1 = c.B

            r2 = 255 - r1
            g2 = 255 - g1
            b2 = 255 - b1

            Return RGBColor(r2, g2, b2)
        End Function

        ''' <summary>
        ''' ScreenMix a color, uses RGBColor and OpacityMix
        ''' </summary>
        ''' <param name="BaseColor"></param>
        ''' <param name="BlendColor"></param>
        ''' <param name="Opacity"></param>
        ''' <returns></returns>
        Private Function ScreenMix(ByVal BaseColor As Color, ByVal BlendColor As Color, ByVal Opacity As Integer) As Color
            Dim r1, g1, b1 As Integer
            Dim r2, g2, b2 As Integer
            Dim r3, g3, b3 As Integer

            r1 = BaseColor.R
            g1 = BaseColor.G
            b1 = BaseColor.B

            r2 = BlendColor.R
            g2 = BlendColor.G
            b2 = BlendColor.B

            r3 = CInt((1 - ((1 - (r1 / 255)) * (1 - (r2 / 255)))) * 255)
            g3 = CInt((1 - ((1 - (g1 / 255)) * (1 - (g2 / 255)))) * 255)
            b3 = CInt((1 - ((1 - (b1 / 255)) * (1 - (b2 / 255)))) * 255)

            Return OpacityMix(RGBColor(r3, g3, b3), BaseColor, Opacity)
        End Function

        ''' <summary>
        ''' MultiplyMix a Color, uses RGBColor and RGBColor
        ''' </summary>
        ''' <param name="BaseColor"></param>
        ''' <param name="BlendColor"></param>
        ''' <param name="Opacity"></param>
        ''' <returns></returns>
        Public Function MultiplyMix(ByVal BaseColor As Color, ByVal BlendColor As Color, ByVal Opacity As Integer) As Color
            Dim r1, g1, b1 As Integer
            Dim r2, g2, b2 As Integer
            Dim r3, g3, b3 As Integer

            r1 = BaseColor.R
            g1 = BaseColor.G
            b1 = BaseColor.B

            r2 = BlendColor.R
            g2 = BlendColor.G
            b2 = BlendColor.B

            r3 = CInt(r1 * r2 / 255)
            g3 = CInt(g1 * g2 / 255)
            b3 = CInt(b1 * b2 / 255)

            Return OpacityMix(RGBColor(r3, g3, b3), BaseColor, Opacity)
        End Function

        ''' <summary>
        ''' SoftLightMix a color, needs AMH_Math.SoftLightMath and RGBColor
        ''' </summary>
        ''' <param name="BaseColor"></param>
        ''' <param name="BlendColor"></param>
        ''' <param name="Opacity"></param>
        ''' <returns></returns>
        Public Function SoftLightMix(ByVal BaseColor As Color, ByVal BlendColor As Color, ByVal Opacity As Integer) As Color
            Dim r1 As Integer, g1 As Integer, b1 As Integer
            Dim r2 As Integer, g2 As Integer, b2 As Integer
            Dim r3 As Integer, g3 As Integer, b3 As Integer

            r1 = BaseColor.R
            g1 = BaseColor.G
            b1 = BaseColor.B

            r2 = BlendColor.R
            g2 = BlendColor.G
            b2 = BlendColor.B

            r3 = Tools.SoftLightMath(r1, r2)
            g3 = Tools.SoftLightMath(g1, g2)
            b3 = Tools.SoftLightMath(b1, b2)

            Return OpacityMix(RGBColor(r3, g3, b3), BaseColor, Opacity)
        End Function

        ''' <summary>
        ''' OverlayMix a Color, need AMH_Math.OverlayMath amd RGBColor
        ''' </summary>
        ''' <param name="BaseColor"></param>
        ''' <param name="BlendColor"></param>
        ''' <param name="opacity"></param>
        ''' <returns></returns>
        Public Function OverlayMix(ByVal BaseColor As Color, ByVal BlendColor As Color, ByVal opacity As Integer) As Color
            Dim r1, g1, b1 As Integer
            Dim r2, g2, b2 As Integer
            Dim r3, g3, b3 As Integer

            r1 = BaseColor.R
            g1 = BaseColor.G
            b1 = BaseColor.B

            r2 = BlendColor.R
            g2 = BlendColor.G
            b2 = BlendColor.B

            r3 = Tools.OverlayMath(BaseColor.R, BlendColor.R)
            g3 = Tools.OverlayMath(BaseColor.G, BlendColor.G)
            b3 = Tools.OverlayMath(BaseColor.B, BlendColor.B)

            Return OpacityMix(RGBColor(r3, g3, b3), BaseColor, opacity)
        End Function

        ''' <summary>
        ''' Creates a panel
        ''' </summary>
        ''' <param name="X"></param>
        ''' <param name="Y"></param>
        ''' <param name="X1"></param>
        ''' <param name="Y1"></param>
        ''' <param name="Inverted"></param>
        Public Sub Panel(X As Long, Y As Long, X1 As Long, Y1 As Long, Optional Inverted As Boolean = False)
            If Not Inverted Then
                For I As Long = 0 To 4
                    Draw.DrawRectangle(X + I, Y + I, X1 - I, Y1 - I, penBlue(CInt(I)))
                Next

                _G.FillRectangle(sbBlue(4), New Rectangle(CInt(X + 4), CInt(Y + 4), CInt((X1 - X) - 8), CInt((Y1 - Y) - 8)))
            Else
                For I As Long = 0 To 4
                    Draw.DrawRectangle(X + I, Y + I, X1 - I, Y1 - I, penBlue(CInt(4 - I)))
                Next

                _G.FillRectangle(sbBlue(0), New Rectangle(CInt(X + 4), CInt(Y + 4), CInt((X1 - X) - 8), CInt((Y1 - Y) - 8)))
            End If
        End Sub

        ''' <summary>
        ''' Creates a round panel
        ''' </summary>
        ''' <param name="X"></param>
        ''' <param name="Y"></param>
        ''' <param name="X1"></param>
        ''' <param name="Y1"></param>
        ''' <param name="Radius"></param>
        Public Overloads Sub RoundPanel(X As Long, Y As Long, X1 As Long, Y1 As Long, Radius As Long)
            RoundFrame(X, Y, X1, Y1, Radius)

            Using gp As New GraphicsPath()
                gp.AddArc((X + 4), (Y + 4), Radius, Radius, 180, 90)
                gp.AddArc(X1 - (Radius + 4), Y + 4, Radius, Radius, 270, 90)
                gp.AddArc(X1 - (Radius + 4), Y1 - (Radius + 4), Radius, Radius, 0, 90)
                gp.AddArc((X + 4), (Y1 - (Radius + 4)), Radius, Radius, 90, 90)

                _G.FillPath(sbBlue(4), gp)
            End Using
        End Sub

        ''' <summary>
        ''' Creates a RoundPanel wuth a Path (usefull to use the click event on the panel)
        ''' </summary>
        ''' <param name="X"></param>
        ''' <param name="Y"></param>
        ''' <param name="X1"></param>
        ''' <param name="Y1"></param>
        ''' <param name="Radius"></param>
        ''' <param name="Path"></param>
        Public Overloads Sub RoundPanel(X As Long, Y As Long, X1 As Long, Y1 As Long, Radius As Long, ByRef Path As GraphicsPath)
            RoundFrame(X, Y, X1, Y1, Radius)

            Using gp As New GraphicsPath()
                gp.AddArc((X + 4), (Y + 4), Radius, Radius, 180, 90)
                gp.AddArc(X1 - (Radius + 4), Y + 4, Radius, Radius, 270, 90)
                gp.AddArc(X1 - (Radius + 4), Y1 - (Radius + 4), Radius, Radius, 0, 90)
                gp.AddArc((X + 4), (Y1 - (Radius + 4)), Radius, Radius, 90, 90)

                _G.FillPath(New SolidBrush(Color.FromArgb(180, 0, 0, 0)), gp)
                Path = gp
            End Using
        End Sub

        ''' <summary>
        ''' Creates and Hexagonal panel
        ''' </summary>
        ''' <param name="X"></param>
        ''' <param name="Y"></param>
        ''' <param name="X1"></param>
        ''' <param name="Y1"></param>
        Public Sub HexagonPanel(X As Long, Y As Long, X1 As Long, Y1 As Long)
            Dim Radius As Long = Tools.RadiusFromTriangle(X, Y, X1, Y1)
            Dim CenterX As Long = CLng((Abs(X - X1) / 2) + X)
            Dim CenterY As Long = CLng((Abs(Y - Y1) / 2) + Y)

            HexagonFrame(X, Y, X1, Y1)

            Using gp As New GraphicsPath()
                gp.AddPolygon({New Point(CInt(X + 4), CInt(Y + 4)),
                       New Point(CInt(X1 - 4), CInt(Y + 4)),
                       New Point(CInt((CenterX + Radius) - 4), CInt(CenterY)),
                       New Point(CInt(X1 - 4), CInt(Y1 - 4)),
                       New Point(CInt(X + 4), CInt(Y1 - 4)),
                       New Point(CInt((CenterX - Radius) + 4), CInt(CenterY))})

                _G.FillPath(sbBlue(4), gp)
            End Using
        End Sub

        ''' <summary>
        ''' Creates the path for round rectangle
        ''' </summary>
        ''' <param name="X"></param>
        ''' <param name="Y"></param>
        ''' <param name="X1"></param>
        ''' <param name="Y1"></param>
        ''' <param name="Radius"></param>
        ''' <returns></returns>
        Public Function RoundRectanglePath(X As Long, Y As Long, X1 As Long, Y1 As Long, Radius As Long) As GraphicsPath
            Dim Path As New GraphicsPath

            With Path
                .AddArc(X1 - Radius, Y1 - Radius, Radius, Radius, 0, 90)
                .AddArc(X, Y1 - Radius, Radius, Radius, 90, 90)
                .AddArc(New Rectangle(CInt(X), CInt(Y), CInt(Radius), CInt(Radius)), 180, 90)
                .AddArc(New Rectangle(CInt(X1 - Radius), CInt(Y), CInt(Radius), CInt(Radius)), 270, 90)
            End With

            Return Path
        End Function
#Region "IDisposable Support"
        Private disposedValue As Boolean

        Protected Overridable Sub Dispose(disposing As Boolean)
            If Not disposedValue Then
                If disposing Then : End If
            End If
            disposedValue = True
        End Sub

        Public Sub Dispose() Implements IDisposable.Dispose
            Dispose(True)
            GC.SuppressFinalize(Me)
        End Sub
#End Region
    End Class
End Namespace