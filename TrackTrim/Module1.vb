Imports System.IO
Imports System.Text
Module Module1

    Sub Main()
        Dim objReader As StreamReader = New StreamReader("in.txt")
        Dim objWriter As StreamWriter = New StreamWriter("out.txt")
        Dim strLine As String
        Dim strTrack As String
        Dim sbEdges As New StringBuilder()
        Dim sglEdgeTime As Single = 0
        Dim intEdgeNum As Integer = 0
        Dim sglEdgeTimeMax As Single = 0

        Dim nonNumericCharacters As New System.Text.RegularExpressions.Regex("\D")

        Do
            strLine = objReader.ReadLine
            If strLine = "" Then Exit Do
            If strLine.TrimStart.StartsWith("<All") Then objWriter.WriteLine(strLine)
            If strLine.TrimStart.StartsWith("<Track") Then
                strTrack = strLine
                Do
                    strLine = objReader.ReadLine
                    If strLine.TrimStart.StartsWith("<Edge") Then
                        sglEdgeTime = Convert.ToSingle(nonNumericCharacters.Replace(strLine.Substring(strLine.IndexOf("EDGE_TIME") + 11, 4), String.Empty))
                        If sglEdgeTime < 24 Then
                            If sglEdgeTime > sglEdgeTimeMax Then sglEdgeTimeMax = sglEdgeTime
                            sbEdges.AppendLine(strLine)
                            intEdgeNum += 1
                        End If
                    ElseIf strLine.StartsWith("</Track") Then
                        sbEdges.AppendLine(strLine)
                        Dim strTrackL As String = strLine.Substring(0, strTrack.IndexOf("TRACK_DURATION") + 15)
                        Dim strTrackM As String = strLine.Substring(strTrack.IndexOf(""" TRACK_START"), strTrack.IndexOf("TRACK_STOP") + 12)
                        Dim strTrackR As String = strLine.Substring(strTrack.IndexOf(""" TRACK_DISPLACEMENT"), strTrack.Length - strTrack.IndexOf(""" TRACK_DISPLACEMENT") + 1)
                        objWriter.WriteLine(strTrackL + (sglEdgeTimeMax + 0.5).ToString + strTrackM + (sglEdgeTimeMax + 0.5).ToString + strTrackR)
                        objWriter.Write(sbEdges)
                        sglEdgeTimeMax = 0
                        Exit Do
                    End If
                Loop
            End If
            If strLine.TrimStart.StartsWith("</All") Then objWriter.WriteLine(strLine)
        Loop
    End Sub

End Module
