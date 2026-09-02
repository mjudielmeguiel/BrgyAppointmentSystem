Imports MySql.Data.MySqlClient

Public Class frmDayAppointmentsList

    Private targetDate As DateTime

    Public Sub New(ByVal selectedDate As DateTime)
        InitializeComponent()
        targetDate = selectedDate
    End Sub

    Private Sub frmDayAppointmentsList_Load(sender As Object, e As EventArgs) Handles MyBase.Load
        lblTitleDate.Text = $"Scheduled Pick-ups for {targetDate:dddd, MMMM dd, yyyy}"
        SetupListView()
        LoadDailyAppointments()
    End Sub

    Private Sub SetupListView()
        lvwAppointments.View = View.Details
        lvwAppointments.FullRowSelect = True
        lvwAppointments.GridLines = True
        lvwAppointments.Columns.Clear()

        lvwAppointments.Columns.Add("Control No.", 110)
        lvwAppointments.Columns.Add("Pick-up Time", 110)
        lvwAppointments.Columns.Add("Resident Name", 200)
        lvwAppointments.Columns.Add("Document / Service", 180)
        lvwAppointments.Columns.Add("Status", 100)
    End Sub

    Private Sub LoadDailyAppointments()
        lvwAppointments.Items.Clear()

        Try
            connection()
            sql = "SELECT ControlNo, ScheduledDate, FullName, RequestType, Status " &
                  "FROM appointments " &
                  "WHERE DATE(ScheduledDate) = @targetDate AND UPPER(Status) != 'CANCELLED' " &
                  "ORDER BY ScheduledDate ASC"

            cmd = New MySqlCommand(sql, cn)
            cmd.Parameters.AddWithValue("@targetDate", targetDate.ToString("yyyy-MM-dd"))
            dr = cmd.ExecuteReader()

            While dr.Read()
                Dim ctrlNo As String = dr("ControlNo").ToString()
                Dim schedTime As String = Convert.ToDateTime(dr("ScheduledDate")).ToString("h:mm tt")
                Dim name As String = dr("FullName").ToString()
                Dim reqType As String = dr("RequestType").ToString()
                Dim status As String = dr("Status").ToString()

                Dim item As New ListViewItem(ctrlNo)
                item.SubItems.Add(schedTime)
                item.SubItems.Add(name)
                item.SubItems.Add(reqType)
                item.SubItems.Add(status)

                lvwAppointments.Items.Add(item)
            End While
            dr.Close()

            If lvwAppointments.Items.Count = 0 Then
                MsgBox("No scheduled pick-ups found for this date.", MsgBoxStyle.Information, "No Appointments")
            End If

        Catch ex As Exception
            MsgBox("Error loading daily list: " & ex.Message, MsgBoxStyle.Critical, "Database Error")
        Finally
            CloseConnection()
        End Try
    End Sub

    Private Sub btnNewAppointment_Click(sender As Object, e As EventArgs) Handles btnNewAppointment.Click
        Using frmCreate As New frmCreateAppointment()
            If frmCreate.ShowDialog() = DialogResult.OK Then
                LoadDailyAppointments()
            End If
        End Using
    End Sub

    Private Sub btnClose_Click(sender As Object, e As EventArgs) Handles btnClose.Click
        Me.Close()
    End Sub

End Class