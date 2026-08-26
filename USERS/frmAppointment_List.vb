Imports MySql.Data.MySqlClient
Imports System.Drawing

Public Class frmAppointment_List

    Private Sub frmAppointment_List_Load(sender As Object, e As EventArgs) Handles MyBase.Load
        SetupDataGridView()
        LoadPendingAppointments()
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
        dgvAppointments.Columns.Add("Status", "STATUS")

        dgvAppointments.Columns("AppointmentID").Width = 80
        dgvAppointments.Columns("IDNumber").Width = 100
        dgvAppointments.Columns("FullName").Width = 180
        dgvAppointments.Columns("Contact").Width = 150
        dgvAppointments.Columns("AppointmentDate").Width = 90
        dgvAppointments.Columns("Purpose").Width = 200
        dgvAppointments.Columns("Status").Width = 90
    End Sub

    Public Sub LoadPendingAppointments()
        Try
            DBconnection.connection()
            dgvAppointments.Rows.Clear()

            Dim sql As String = "SELECT AppointmentID, IDNumber, FullName, EmailAddress, PhoneNumber, AppointmentDate, Purpose, Status FROM appointments WHERE Status = 'PENDING' ORDER BY CreatedAt DESC"

            Using cmd As New MySqlCommand(sql, DBconnection.cn)
                Using dr As MySqlDataReader = cmd.ExecuteReader()
                    While dr.Read()
                        Dim contactInfo As String = dr("EmailAddress").ToString() & vbCrLf & dr("PhoneNumber").ToString()

                        dgvAppointments.Rows.Add(
                            dr("AppointmentID").ToString(),
                            dr("IDNumber").ToString(),
                            dr("FullName").ToString(),
                            contactInfo,
                            Convert.ToDateTime(dr("AppointmentDate")).ToString("MM/dd/yyyy"),
                            dr("Purpose").ToString(),
                            dr("Status").ToString().ToUpper()
                        )
                    End While
                End Using
            End Using

        Catch ex As Exception
            MessageBox.Show("ERROR LOADING LIST: " & ex.Message, "ERROR", MessageBoxButtons.OK, MessageBoxIcon.Error)
        Finally
            DBconnection.CloseConnection()
        End Try
    End Sub

    Private Sub txtSearch_TextChanged(sender As Object, e As EventArgs) Handles txtSearch.TextChanged
        Dim keyword As String = txtSearch.Text.Trim()
        If keyword.Length < 2 Then
            LoadPendingAppointments()
            Return
        End If

        Try
            DBconnection.connection()
            dgvAppointments.Rows.Clear()

            Dim sql As String = "SELECT AppointmentID, IDNumber, FullName, EmailAddress, PhoneNumber, AppointmentDate, Purpose, Status FROM appointments WHERE Status = 'PENDING' AND (FullName LIKE @Keyword OR IDNumber LIKE @Keyword) ORDER BY CreatedAt DESC"

            Using cmd As New MySqlCommand(sql, DBconnection.cn)
                cmd.Parameters.AddWithValue("@Keyword", "%" & keyword & "%")
                Using dr As MySqlDataReader = cmd.ExecuteReader()
                    While dr.Read()
                        Dim contactInfo As String = dr("EmailAddress").ToString() & vbCrLf & dr("PhoneNumber").ToString()
                        dgvAppointments.Rows.Add(
                            dr("AppointmentID").ToString(),
                            dr("IDNumber").ToString(),
                            dr("FullName").ToString(),
                            contactInfo,
                            Convert.ToDateTime(dr("AppointmentDate")).ToString("MM/dd/yyyy"),
                            dr("Purpose").ToString(),
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

    Private Sub btnRefresh_Click(sender As Object, e As EventArgs) Handles btnRefresh.Click
        txtSearch.Clear()
        LoadPendingAppointments()
    End Sub

    Private Sub btnAddNewRecord_Click(sender As Object, e As EventArgs) Handles btnAddNewRecord.Click
        Dim frm As New frmBookAppointment()
        If frm.ShowDialog() = DialogResult.OK Then
            LoadPendingAppointments()
        End If
    End Sub

    Private Sub btnSetAppointment_Click(sender As Object, e As EventArgs) Handles btnSetAppointment.Click
        Dim frm As New frmBook()
        If frm.ShowDialog() = DialogResult.OK Then
            LoadPendingAppointments()
        End If
    End Sub
End Class