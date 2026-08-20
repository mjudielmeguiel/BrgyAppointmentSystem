Imports System.Drawing
Imports System.Text
Imports MySql.Data.MySqlClient

Public Class frmDocumentServices

    ' ✅ IPAPASA BALIK SA IBA PANG FORM
    Public Property SelectedCodes As String = ""
    Public Property SelectedNames As String = ""
    Public Property TotalAmount As Decimal = 0.00D

    ' ✅ ITATAGO NATIN ANG ORIHINAL NA CODE PAG NAG-EDIT KA
    Private originalCode As String = ""

    Private Sub frmDocumentServices_Load(sender As Object, e As EventArgs) Handles MyBase.Load
        ' ✅ AYUSIN ANG LISTVIEW
        lvServices.View = View.Details
        lvServices.FullRowSelect = True
        lvServices.GridLines = False
        lvServices.Columns.Clear()
        lvServices.Columns.Add("Code", 70)
        lvServices.Columns.Add("Document / Service", 320)
        lvServices.Columns.Add("Amount", 90)

        LoadServicesToList()
    End Sub

    ' ✅ KUKUHA LAHAT NG DOKUMENTO MULA SA TABLE
    Private Sub LoadServicesToList()
        Try
            DBconnection.connection()
            lvServices.Items.Clear()

            Using cmd As New MySqlCommand("SELECT ServiceCode, ServiceName, Amount FROM document_services WHERE IsActive = 1 ORDER BY ServiceName", DBconnection.cn)
                Using dr As MySqlDataReader = cmd.ExecuteReader()
                    While dr.Read()
                        Dim code = dr("ServiceCode").ToString()
                        Dim name = dr("ServiceName").ToString()
                        Dim amount = Convert.ToDecimal(dr("Amount"))

                        Dim item As New ListViewItem(code)
                        item.SubItems.Add(name)
                        item.SubItems.Add("₱ " & amount.ToString("F2"))
                        item.Tag = New With {Key .Code = code, Key .Name = name, Key .Amount = amount}
                        lvServices.Items.Add(item)
                    End While
                End Using
            End Using

        Catch ex As Exception
            MessageBox.Show("ERROR LOADING LIST: " & ex.Message, "ERROR", MessageBoxButtons.OK, MessageBoxIcon.Error)
        Finally
            DBconnection.CloseConnection()
        End Try
    End Sub

    ' ✅ TUWING MAY PINILI — LALABAS SA BABA AT PWEDENG I-EDIT
    Private Sub lvServices_SelectedIndexChanged(sender As Object, e As EventArgs) Handles lvServices.SelectedIndexChanged
        If lvServices.SelectedItems.Count = 0 Then
            txtServiceName.Clear()
            txtAmount.Clear()
            lblServiceCode.Text = "Service Code — (AWTO)"
            originalCode = ""
            Exit Sub
        End If

        Dim selectedItem = lvServices.SelectedItems(0)
        originalCode = selectedItem.Text  ' ✅ ORIHINAL NA CODE — HINDI NAGBABAGO
        Dim name As String = selectedItem.SubItems(1).Text
        Dim amountText As String = selectedItem.SubItems(2).Text

        lblServiceCode.Text = "Service Code — " & originalCode
        txtServiceName.Text = name
        txtAmount.Text = amountText.Replace("₱ ", "")
    End Sub

    ' ✅ TUWING NAGTITIPA KA SA SERVICE NAME — AWTO-NAKIKITA ANG CODE!
    Private Sub txtServiceName_TextChanged(sender As Object, e As EventArgs) Handles txtServiceName.TextChanged
        If String.IsNullOrWhiteSpace(originalCode) AndAlso Not String.IsNullOrWhiteSpace(txtServiceName.Text) Then
            ' ✅ BAGONG DOKUMENTO — I-SHOW ANG AWTO-GENERATED CODE
            Dim autoCode As String = GenerateCodeFromName(txtServiceName.Text)
            lblServiceCode.Text = "Service Code — " & autoCode & " (AWTO)"
        ElseIf String.IsNullOrWhiteSpace(txtServiceName.Text) Then
            lblServiceCode.Text = "Service Code — (AWTO)"
        End If
    End Sub

    ' ✅ ANG MAHIRAP NA GAWAIN: GUMAGAWA NG CODE MULA SA PANGALAN
    Private Function GenerateCodeFromName(fullName As String) As String
        If String.IsNullOrWhiteSpace(fullName) Then Return ""

        ' ✅ HIWALAYIN ANG MGA SALITA
        Dim words As String() = fullName.ToUpper().Split({" "c}, StringSplitOptions.RemoveEmptyEntries)
        Dim code As New StringBuilder()

        ' ✅ KUNG ISANG SALITA LANG — KUNIN ANG UNA APAT NA TITIK
        If words.Length = 1 Then
            Dim word = words(0).Trim()
            Return If(word.Length >= 4, word.Substring(0, 4), word.PadRight(4, "X"c)).Substring(0, 4)
        End If

        ' ✅ KUNG MARAMING SALITA — KUNIN ANG UNA TITIK NG BAWAT SALITA
        For Each word In words
            Dim clean = word.Trim()
            If clean.Length > 0 AndAlso Not {"OF", "THE", "AND", "FOR"}.Contains(clean) Then
                code.Append(clean(0))
            End If
        Next

        ' ✅ KUNG KULANG SA APAT — DAGDAGAN MULA SA UNANG SALITA
        If code.Length < 4 AndAlso words(0).Length >= code.Length Then
            For i As Integer = code.Length To Math.Min(words(0).Length - 1, 3)
                code.Append(words(0)(i))
            Next
        End If

        ' ✅ SIGURADUHIN NA 4 TITIK LANG
        Dim result = code.ToString().Replace(" ", "").Substring(0, Math.Min(code.Length, 4))
        Return result
    End Function

    ' ✅ CLEAR BUTTON — PARA SA BAGONG DOKUMENTO
    Private Sub btnClear_Click(sender As Object, e As EventArgs) Handles btnClear.Click
        lvServices.SelectedItems.Clear()
        txtServiceName.Clear()
        txtAmount.Clear()
        lblServiceCode.Text = "Service Code — (AWTO)"
        originalCode = ""
        txtServiceName.Focus()
    End Sub

    ' ✅ SAVE BUTTON — ADD O EDIT
    Private Sub btnSave_Click(sender As Object, e As EventArgs) Handles btnSave.Click
        ' ✅ KUNG WALANG LAMANG PANGALAN O HALAGA
        If String.IsNullOrWhiteSpace(txtServiceName.Text) Or String.IsNullOrWhiteSpace(txtAmount.Text) Then
            MessageBox.Show("ILAGAY ANG PANGALAN AT HALAGA.", "PAALALA", MessageBoxButtons.OK, MessageBoxIcon.Information)
            Return
        End If

        Try
            DBconnection.connection()

            If String.IsNullOrWhiteSpace(originalCode) Then
                ' ==============================================
                ' ✅ BAGONG DOKUMENTO — AWTO NA ANG CODE!
                ' ==============================================
                Dim newCode As String = GenerateCodeFromName(txtServiceName.Text.Trim())

                ' ✅ SURIIN KUNG MAY KAPAREHAS NA — DAGDAGAN NG NUMERO KUNG MERON
                Dim finalCode As String = newCode
                Dim counter As Integer = 1
                Using cmdCheck As New MySqlCommand("SELECT COUNT(*) FROM document_services WHERE ServiceCode = @Code", DBconnection.cn)
                    Do
                        cmdCheck.Parameters.Clear()
                        cmdCheck.Parameters.AddWithValue("@Code", finalCode)
                        Dim exists As Integer = Convert.ToInt32(cmdCheck.ExecuteScalar())
                        If exists = 0 Then Exit Do
                        finalCode = newCode & counter.ToString()(0)
                        counter += 1
                    Loop
                End Using

                ' ✅ I-INSERT SA DATABASE — AWTO NA ANG CODE!
                Using cmd As New MySqlCommand("INSERT INTO document_services (ServiceCode, ServiceName, Amount) VALUES (@Code, @Name, @Amount)", DBconnection.cn)
                    cmd.Parameters.AddWithValue("@Code", finalCode)
                    cmd.Parameters.AddWithValue("@Name", txtServiceName.Text.Trim())
                    cmd.Parameters.AddWithValue("@Amount", Decimal.Parse(txtAmount.Text.Trim()))
                    cmd.ExecuteNonQuery()
                End Using

                MessageBox.Show("NADAGDAG! CODE: " & finalCode, "SUCCESS", MessageBoxButtons.OK, MessageBoxIcon.Information)

            Else
                ' ==============================================
                ' ✅ MAY PINILI = I-UPDATE / I-EDIT — HINDI BABAGUHIN ANG CODE!
                ' ==============================================
                Using cmd As New MySqlCommand("UPDATE document_services SET ServiceName = @Name, Amount = @Amount WHERE ServiceCode = @Code", DBconnection.cn)
                    cmd.Parameters.AddWithValue("@Name", txtServiceName.Text.Trim())
                    cmd.Parameters.AddWithValue("@Amount", Decimal.Parse(txtAmount.Text.Trim()))
                    cmd.Parameters.AddWithValue("@Code", originalCode)
                    cmd.ExecuteNonQuery()
                End Using

                MessageBox.Show("NAI-UPDATE NA! CODE: " & originalCode, "SUCCESS", MessageBoxButtons.OK, MessageBoxIcon.Information)

            End If

            ' ✅ AUTO-REFRESH — AGAD LALABAS ANG PAGBABAGO
            LoadServicesToList()

            ' ✅ I-CLEAR ANG FORM
            btnClear_Click(sender, e)

        Catch ex As Exception
            MessageBox.Show("ERROR: " & ex.Message, "ERROR", MessageBoxButtons.OK, MessageBoxIcon.Error)
        Finally
            DBconnection.CloseConnection()
        End Try
    End Sub

    ' ==============================================
    ' ✅ DELETE BUTTON — BURAHIN ANG PINILI
    ' ==============================================
    Private Sub btnDelete_Click(sender As Object, e As EventArgs) Handles btnDelete.Click
        If lvServices.SelectedItems.Count = 0 Then
            MessageBox.Show("PUMILI MUNA NG DOKUMENTO NA BURAHIN.", "PAALALA", MessageBoxButtons.OK, MessageBoxIcon.Information)
            Return
        End If

        Dim selectedCode As String = lvServices.SelectedItems(0).Text
        Dim selectedName As String = lvServices.SelectedItems(0).SubItems(1).Text

        ' ✅ KUMPIRMASYON BAGO BURAHIN
        If MessageBox.Show("BURAHIN BA TALAGA? " & selectedName & vbCrLf & "CODE: " & selectedCode, "KUMPIRMA", MessageBoxButtons.YesNo, MessageBoxIcon.Question) <> DialogResult.Yes Then
            Return
        End If

        Try
            DBconnection.connection()

            ' ✅ OPTION 1: TALAGANG BURAHIN SA DATABASE
            ' Using cmd As New MySqlCommand("DELETE FROM document_services WHERE ServiceCode = @Code", DBconnection.cn)

            ' ✅ OPTION 2: ITAGO LANG (HINDI NA LALABAS) — MAS LIGTAS!
            Using cmd As New MySqlCommand("UPDATE document_services SET IsActive = 0 WHERE ServiceCode = @Code", DBconnection.cn)
                cmd.Parameters.AddWithValue("@Code", selectedCode)
                cmd.ExecuteNonQuery()
            End Using

            MessageBox.Show("NABURAHIN NA! CODE: " & selectedCode, "SUCCESS", MessageBoxButtons.OK, MessageBoxIcon.Information)

            ' ✅ AUTO-REFRESH — AGAD MAWAWALA SA LISTAHAN
            LoadServicesToList()

            ' ✅ I-CLEAR ANG FORM
            btnClear_Click(sender, e)

        Catch ex As Exception
            MessageBox.Show("ERROR DELETING: " & ex.Message, "ERROR", MessageBoxButtons.OK, MessageBoxIcon.Error)
        Finally
            DBconnection.CloseConnection()
        End Try
    End Sub

    ' ✅ CLOSE BUTTON
    Private Sub btnClose_Click(sender As Object, e As EventArgs) Handles btnClose.Click
        Me.DialogResult = DialogResult.Cancel
        Me.Close()
    End Sub

End Class