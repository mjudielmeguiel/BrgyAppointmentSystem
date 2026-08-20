Imports MySql.Data.MySqlClient
Imports System.Drawing

Public Class frmAppointment_List

    Private Sub frmAppointment_List_Load(sender As Object, e As EventArgs) Handles MyBase.Load
        SetupDataGridView()
        LoadAppointments()
        UpdateStatusCounts()
    End Sub

    Private Sub SetupDataGridView()
        dgvAppointments.AutoGenerateColumns = False
        dgvAppointments.AllowUserToAddRows = False
        dgvAppointments.ReadOnly = True
        dgvAppointments.RowHeadersVisible = False
        dgvAppointments.SelectionMode = DataGridViewSelectionMode.FullRowSelect

        dgvAppointments.Columns.Clear()
        dgvAppointments.Columns.Add("AppointmentID", "APP'T NO.")
        dgvAppointments.Columns.Add("IDNumber", "RESIDENT ID")
        dgvAppointments.Columns.Add("FullName", "FULL NAME")
        dgvAppointments.Columns.Add("Contact", "CONTACT INFO")
        dgvAppointments.Columns.Add("AppointmentDate", "DATE")
        dgvAppointments.Columns.Add("Purpose", "PURPOSE / SERVICES")
        dgvAppointments.Columns.Add("TotalAmount", "TOTAL")
        dgvAppointments.Columns.Add("Status", "STATUS")

        dgvAppointments.Columns("AppointmentID").Width = 80
        dgvAppointments.Columns("IDNumber").Width = 100
        dgvAppointments.Columns("FullName").Width = 180
        dgvAppointments.Columns("Contact").Width = 150
        dgvAppointments.Columns("AppointmentDate").Width = 90
        dgvAppointments.Columns("Purpose").Width = 200
        dgvAppointments.Columns("TotalAmount").Width = 80
        dgvAppointments.Columns("Status").Width = 90
    End Sub

    Public Sub LoadAppointments(Optional filterStatus As String = "")
        Try
            DBconnection.connection()
            dgvAppointments.Rows.Clear()

            Dim sql As String = "SELECT AppointmentID, IDNumber, FullName, EmailAddress, PhoneNumber, AppointmentDate, ServiceNames, TotalAmount, Status FROM appointments WHERE Status <> 'RELEASED' ORDER BY CreatedAt DESC"

            If Not String.IsNullOrWhiteSpace(filterStatus) Then
                sql = "SELECT AppointmentID, IDNumber, FullName, EmailAddress, PhoneNumber, AppointmentDate, ServiceNames, TotalAmount, Status FROM appointments WHERE Status = @Status ORDER BY CreatedAt DESC"
            End If

            Using cmd As New MySqlCommand(sql, DBconnection.cn)
                If Not String.IsNullOrWhiteSpace(filterStatus) Then
                    cmd.Parameters.AddWithValue("@Status", filterStatus)
                End If

                Using dr As MySqlDataReader = cmd.ExecuteReader()
                    While dr.Read()
                        Dim contactInfo As String = dr("EmailAddress").ToString() & vbCrLf & dr("PhoneNumber").ToString()
                        Dim totalAmt As String = If(Convert.ToDecimal(dr("TotalAmount")) > 0, "₱ " & Convert.ToDecimal(dr("TotalAmount")).ToString("F2"), "")
                        Dim services As String = dr("ServiceNames").ToString()
                        If String.IsNullOrWhiteSpace(services) Then services = dr("ServiceNames").ToString()

                        dgvAppointments.Rows.Add(
                            dr("AppointmentID").ToString(),
                            dr("IDNumber").ToString(),
                            dr("FullName").ToString(),
                            contactInfo,
                            Convert.ToDateTime(dr("AppointmentDate")).ToString("MM/dd/yyyy"),
                            services,
                            totalAmt,
                            dr("Status").ToString().ToUpper()
                        )
                    End While
                End Using
            End Using

            UpdateStatusCounts()

        Catch ex As Exception
            MessageBox.Show("ERROR LOADING LIST: " & ex.Message, "ERROR", MessageBoxButtons.OK, MessageBoxIcon.Error)
        Finally
            DBconnection.CloseConnection()
        End Try
    End Sub

    ' ✅ DOUBLE CLICK SA ROW → BUBUKAS ANG TRANSACTION FORM!
    Private Sub dgvAppointments_CellDoubleClick(sender As Object, e As DataGridViewCellEventArgs) Handles dgvAppointments.CellDoubleClick
        If e.RowIndex < 0 Then Exit Sub

        Dim appID As Integer = Integer.Parse(dgvAppointments.Rows(e.RowIndex).Cells("AppointmentID").Value.ToString())

        ' ✅ BUKASIN ANG TRANSACTION FORM KASAMA ANG DETALYE
        Dim frm As New frmAppointment_Transaction(appID)
        If frm.ShowDialog() = DialogResult.OK Then
            LoadAppointments()
        End If
    End Sub

    Private Sub UpdateStatusCounts()
        Try
            DBconnection.connection()
            Dim pendingCount As Integer = 0
            Dim paidCount As Integer = 0
            Dim releasedCount As Integer = 0

            Using cmd As New MySqlCommand("SELECT COUNT(*) FROM appointments WHERE Status = 'PENDING'", DBconnection.cn)
                pendingCount = Convert.ToInt32(cmd.ExecuteScalar())
            End Using
            Using cmd As New MySqlCommand("SELECT COUNT(*) FROM appointments WHERE Status = 'PAID'", DBconnection.cn)
                paidCount = Convert.ToInt32(cmd.ExecuteScalar())
            End Using
            Using cmd As New MySqlCommand("SELECT COUNT(*) FROM appointments WHERE Status = 'RELEASED'", DBconnection.cn)
                releasedCount = Convert.ToInt32(cmd.ExecuteScalar())
            End Using

            lblPending.Text = "PENDING: " & pendingCount
            lblPaid.Text = "PAID: " & paidCount
            lblReleased.Text = "RELEASED: " & releasedCount

            lblPending.ForeColor = Color.Orange
            lblPaid.ForeColor = Color.Green
            lblReleased.ForeColor = Color.Blue

        Catch ex As Exception
        Finally
            DBconnection.CloseConnection()
        End Try
    End Sub

    Private Sub txtSearch_TextChanged(sender As Object, e As EventArgs) Handles txtSearch.TextChanged
        Dim keyword As String = txtSearch.Text.Trim()
        If keyword.Length < 2 Then
            LoadAppointments()
            Return
        End If

        Try
            DBconnection.connection()
            dgvAppointments.Rows.Clear()

            Dim sql As String = "SELECT AppointmentID, IDNumber, FullName, EmailAddress, PhoneNumber, AppointmentDate, ServiceNames, TotalAmount, Status FROM appointments WHERE Status <> 'RELEASED' AND (FullName LIKE @Keyword OR IDNumber LIKE @Keyword) ORDER BY CreatedAt DESC"

            Using cmd As New MySqlCommand(sql, DBconnection.cn)
                cmd.Parameters.AddWithValue("@Keyword", "%" & keyword & "%")
                Using dr As MySqlDataReader = cmd.ExecuteReader()
                    While dr.Read()
                        Dim contactInfo As String = dr("EmailAddress").ToString() & vbCrLf & dr("PhoneNumber").ToString()
                        Dim totalAmt As String = If(Convert.ToDecimal(dr("TotalAmount")) > 0, "₱ " & Convert.ToDecimal(dr("TotalAmount")).ToString("F2"), "")
                        dgvAppointments.Rows.Add(
                            dr("AppointmentID").ToString(),
                            dr("IDNumber").ToString(),
                            dr("FullName").ToString(),
                            contactInfo,
                            Convert.ToDateTime(dr("AppointmentDate")).ToString("MM/dd/yyyy"),
                            dr("ServiceNames").ToString(),
                            totalAmt,
                            dr("Status").ToString().ToUpper()
                        )
                    End While
                End Using
            End Using
        Catch ex As Exception
        Finally
            DBconnection.CloseConnection()
        End Try
    End Sub

    Private Sub lblPending_Click(sender As Object, e As EventArgs) Handles lblPending.Click
        LoadAppointments("PENDING")
    End Sub

    Private Sub lblPaid_Click(sender As Object, e As EventArgs) Handles lblPaid.Click
        LoadAppointments("PAID")
    End Sub

    Private Sub lblReleased_Click(sender As Object, e As EventArgs) Handles lblReleased.Click
        LoadAppointments("RELEASED")
    End Sub

    Private Sub btnRefresh_Click(sender As Object, e As EventArgs) Handles btnRefresh.Click
        txtSearch.Clear()
        LoadAppointments()
    End Sub

    Private Sub btnPaid_Click(sender As Object, e As EventArgs) Handles btnPaid.Click
        If dgvAppointments.SelectedRows.Count = 0 Then
            MessageBox.Show("PUMILI MUNA NG RECORD SA LISTAHAN.", "PAALALA", MessageBoxButtons.OK, MessageBoxIcon.Information)
            Return
        End If

        Dim appID As Integer = Integer.Parse(dgvAppointments.SelectedRows(0).Cells("AppointmentID").Value.ToString())
        If MessageBox.Show("MARK AS PAID? CONFIRM UPDATE?", "CONFIRM", MessageBoxButtons.YesNo, MessageBoxIcon.Question) = DialogResult.Yes Then
            UpdateStatus(appID, "PAID")
        End If
    End Sub

    Private Sub btnReleasing_Click(sender As Object, e As EventArgs) Handles btnReleasing.Click
        If dgvAppointments.SelectedRows.Count = 0 Then
            MessageBox.Show("PUMILI MUNA NG RECORD SA LISTAHAN.", "PAALALA", MessageBoxButtons.OK, MessageBoxIcon.Information)
            Return
        End If

        Dim appID As Integer = Integer.Parse(dgvAppointments.SelectedRows(0).Cells("AppointmentID").Value.ToString())
        If MessageBox.Show("MARK AS RELEASED? CONFIRM UPDATE?", "CONFIRM", MessageBoxButtons.YesNo, MessageBoxIcon.Question) = DialogResult.Yes Then
            UpdateStatus(appID, "RELEASED")
        End If
    End Sub

    Private Sub UpdateStatus(appointmentID As Integer, newStatus As String)
        Try
            DBconnection.connection()
            Using cmd As New MySqlCommand("UPDATE appointments SET Status = @Status WHERE AppointmentID = @ID", DBconnection.cn)
                cmd.Parameters.AddWithValue("@Status", newStatus)
                cmd.Parameters.AddWithValue("@ID", appointmentID)
                cmd.ExecuteNonQuery()
            End Using
            MessageBox.Show("STATUS UPDATED TO " & newStatus & "!", "SUCCESS", MessageBoxButtons.OK, MessageBoxIcon.Information)
            LoadAppointments()
        Catch ex As Exception
            MessageBox.Show("ERROR UPDATING STATUS: " & ex.Message, "ERROR", MessageBoxButtons.OK, MessageBoxIcon.Error)
        Finally
            DBconnection.CloseConnection()
        End Try
    End Sub

    Private Sub btnAddNewRecord_Click(sender As Object, e As EventArgs) Handles btnAddNewRecord.Click
        Dim frm As New frmBookAppointment()
        If frm.ShowDialog() = DialogResult.OK Then
            LoadAppointments()
        End If
    End Sub

    Private Sub btnSetAppointment_Click(sender As Object, e As EventArgs) Handles btnSetAppointment.Click
        Dim frm As New frmBook()
        If frm.ShowDialog() = DialogResult.OK Then
            LoadAppointments()
        End If
    End Sub

End Class