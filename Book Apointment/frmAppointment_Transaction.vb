Imports System.Drawing
Imports MySql.Data.MySqlClient

Public Class frmAppointment_Transaction

    Private ReadOnly appointmentID As Integer = 0

    ' ✅ KAPAG BINUKSAN - TANGGAPIN ANG APPOINTMENT ID
    Public Sub New(appID As Integer)
        InitializeComponent()
        appointmentID = appID
    End Sub

    Private Sub frmAppointment_Transaction_Load(sender As Object, e As EventArgs) Handles MyBase.Load
        lblAppointmentID.Text = $"Appointment ID - {appointmentID}"
        LoadAppointmentDetails()
        LoadServiceComboBox()
        UpdateTotals()
    End Sub

    ' ✅ KUKUHA NG DETALYE NG APPOINTMENT MULA SA DATABASE
    Private Sub LoadAppointmentDetails()
        Try
            DBconnection.connection()

            Using cmd As New MySqlCommand("SELECT FullName, AppointmentDate, ServiceCodes, ServiceNames, TotalAmount, Status FROM appointments WHERE AppointmentID = @ID", DBconnection.cn)
                cmd.Parameters.AddWithValue("@ID", appointmentID)

                Using dr As MySqlDataReader = cmd.ExecuteReader()
                    If dr.Read() Then
                        lblFullName.Text = dr("FullName").ToString()
                        lblDate.Text = Convert.ToDateTime(dr("AppointmentDate")).ToString("MM/dd/yyyy")
                        lblServiceCodes.Text = dr("ServiceCodes").ToString()
                        txtServices.Text = dr("ServiceNames").ToString()
                        lblTotalAmount.Text = $"₱ {Convert.ToDecimal(dr("TotalAmount")):F2}"
                        lblStatus.Text = dr("Status").ToString().ToUpper()
                    End If
                End Using
            End Using

        Catch ex As Exception
            MessageBox.Show($"ERROR LOADING DETAILS: {ex.Message}", "ERROR", MessageBoxButtons.OK, MessageBoxIcon.Error)
        Finally
            DBconnection.CloseConnection()
        End Try
    End Sub

    ' ✅ I-LOAD ANG LAHAT NG DOKUMENTO SA COMBO BOX
    Private Sub LoadServiceComboBox()
        Try
            DBconnection.connection()
            cboService.Items.Clear()
            cboService.Items.Add("-- SELECT DOCUMENT --")

            Using cmd As New MySqlCommand("SELECT ServiceCode, ServiceName, Amount FROM document_services WHERE IsActive = 1 ORDER BY ServiceName", DBconnection.cn)
                Using dr As MySqlDataReader = cmd.ExecuteReader()
                    While dr.Read()
                        Dim code = dr("ServiceCode").ToString()
                        Dim name = dr("ServiceName").ToString()
                        Dim amount = Convert.ToDecimal(dr("Amount"))
                        Dim displayText = $"[{code}] {name} - ₱ {amount:F2}"

                        cboService.Items.Add(New ServiceItem(displayText, code, name, amount))
                    End While
                End Using
            End Using

            cboService.SelectedIndex = 0

        Catch ex As Exception
            MessageBox.Show($"ERROR LOADING DOCUMENTS: {ex.Message}", "ERROR", MessageBoxButtons.OK, MessageBoxIcon.Error)
        Finally
            DBconnection.CloseConnection()
        End Try
    End Sub

    ' ✅ DAGDAGIN ANG NAPILING DOKUMENTO
    Private Sub btnAddService_Click(sender As Object, e As EventArgs) Handles btnAddService.Click
        If cboService.SelectedIndex <= 0 Then
            MessageBox.Show("PUMILI MUNA NG DOKUMENTO SA LISTAHAN.", "PAALALA", MessageBoxButtons.OK, MessageBoxIcon.Information)
            Return
        End If

        Dim selectedItem As ServiceItem = CType(cboService.SelectedItem, ServiceItem)
        Dim currentNames = If(String.IsNullOrWhiteSpace(txtServices.Text), "", $"{txtServices.Text}, ")
        Dim currentCodes = ExtractCodes()
        Dim newCodes = If(String.IsNullOrWhiteSpace(currentCodes), selectedItem.Code, $"{currentCodes}, {selectedItem.Code}")
        Dim newNames = $"{currentNames}{selectedItem.Name}"

        txtServices.Text = newNames
        lblServiceCodes.Text = $"Codes - {newCodes}"

        UpdateTotals()
    End Sub

    Private Sub UpdateTotals()
        Dim codeList = GetCodeList()
        Dim totalDocs = codeList.Count
        Dim totalAmt = GetTotalAmountFromCodes(codeList)

        lblTotalDocs.Text = totalDocs.ToString()
        lblTotalAmount.Text = $"₱ {totalAmt:F2}"
    End Sub

    Private Function GetCodeList() As List(Of String)
        Dim codes = ExtractCodes()
        If String.IsNullOrWhiteSpace(codes) Then Return New List(Of String)()

        Return codes.Split(","c).Select(Function(c) c.Trim()).Where(Function(c) Not String.IsNullOrWhiteSpace(c)).ToList()
    End Function

    Private Function GetTotalAmountFromCodes(codes As List(Of String)) As Decimal
        Dim total As Decimal = 0D

        If codes.Count = 0 Then Return total

        Try
            DBconnection.connection()
            For Each code In codes
                Using cmd As New MySqlCommand("SELECT Amount FROM document_services WHERE ServiceCode = @Code", DBconnection.cn)
                    cmd.Parameters.AddWithValue("@Code", code)
                    Dim result = cmd.ExecuteScalar()
                    If result IsNot Nothing Then
                        total += Convert.ToDecimal(result)
                    End If
                End Using
            Next
        Catch ex As Exception
            MessageBox.Show($"ERROR CALCULATING TOTAL: {ex.Message}", "ERROR", MessageBoxButtons.OK, MessageBoxIcon.Error)
        Finally
            DBconnection.CloseConnection()
        End Try

        Return total
    End Function

    Private Function ExtractCodes() As String
        Dim fullText = lblServiceCodes.Text
        If fullText.Contains("- ") Then
            Return fullText.Split("-"c)(1).Trim()
        End If
        Return String.Empty
    End Function

    Private Sub btnSubmit_Click(sender As Object, e As EventArgs) Handles btnSubmit.Click
        Try
            Dim codes = ExtractCodes()
            If String.IsNullOrWhiteSpace(codes) Then
                MessageBox.Show("WALANG DOKUMENTONG ILALAGAY.", "PAALALA", MessageBoxButtons.OK, MessageBoxIcon.Information)
                Return
            End If

            Dim totalAmt = Decimal.Parse(lblTotalAmount.Text.Replace("₱ ", "").Trim())

            DBconnection.connection()
            Using cmd As New MySqlCommand("UPDATE appointments SET ServiceCodes = @Codes, ServiceNames = @Names, TotalAmount = @Total WHERE AppointmentID = @ID", DBconnection.cn)
                cmd.Parameters.AddWithValue("@Codes", codes)
                cmd.Parameters.AddWithValue("@Names", txtServices.Text.Trim())
                cmd.Parameters.AddWithValue("@Total", totalAmt)
                cmd.Parameters.AddWithValue("@ID", appointmentID)
                cmd.ExecuteNonQuery()
            End Using

            MessageBox.Show("NAISAVE NA! ✅", "SUCCESS", MessageBoxButtons.OK, MessageBoxIcon.Information)
            Me.DialogResult = DialogResult.OK
            Me.Close()

        Catch ex As Exception
            MessageBox.Show($"ERROR SAVING: {ex.Message}", "ERROR", MessageBoxButtons.OK, MessageBoxIcon.Error)
        Finally
            DBconnection.CloseConnection()
        End Try
    End Sub

    Private Sub btnClose_Click(sender As Object, e As EventArgs) Handles btnClose.Click
        Me.DialogResult = DialogResult.Cancel
        Me.Close()
    End Sub

End Class

Public Class ServiceItem
    Public Property DisplayText As String
    Public Property Code As String
    Public Property Name As String
    Public Property Amount As Decimal

    Public Sub New(displayText As String, svcCode As String, svcName As String, svcAmount As Decimal)
        displayText = displayText
        Code = svcCode
        Name = svcName
        Amount = svcAmount
    End Sub

    Public Overrides Function ToString() As String
        Return DisplayText
    End Function
End Class