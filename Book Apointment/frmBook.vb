Imports MySql.Data.MySqlClient
Imports System.Drawing
Imports System.IO

Public Class frmBook

    Private Sub frmBook_Load(sender As Object, e As EventArgs) Handles MyBase.Load
        ShowLabels()
        LoadPurposeComboBox()
    End Sub

    Private Sub LoadPurposeComboBox()
        cboPurpose.Items.Clear()
        cboPurpose.Items.Add("BARANGAY CLEARANCE")
        cboPurpose.Items.Add("BARANGAY ID")
        cboPurpose.Items.Add("CERTIFICATE OF RESIDENCY")
        cboPurpose.Items.Add("CERTIFICATE OF GOOD MORAL CHARACTER")
        cboPurpose.Items.Add("INDIGENCY CERTIFICATE")
        cboPurpose.Items.Add("LIVING CERTIFICATE")
        cboPurpose.Items.Add("OATH OF UNDERTAKING")
        cboPurpose.Items.Add("PERMIT / ENDORSEMENT")
        cboPurpose.Items.Add("BUSINESS CLEARANCE")
        cboPurpose.Items.Add("SOLICITATION PERMIT")
        cboPurpose.Items.Add("TRANSFER OF RESIDENCY")
        cboPurpose.Items.Add("OTHER PURPOSE")

        If cboPurpose.Items.Count > 0 Then
            cboPurpose.SelectedIndex = 0
        End If
    End Sub

    Public Sub LoadResidentData(idNumber As String)
        Me.Text = "RESIDENT INFORMATION"

        Try
            DBconnection.connection()

            Using cmd As New MySqlCommand("SELECT * FROM residents WHERE IDNumber = @ResID LIMIT 1", DBconnection.cn)
                cmd.Parameters.AddWithValue("@ResID", idNumber)

                Using dr As MySqlDataReader = cmd.ExecuteReader()
                    If dr.Read() Then

                        Dim addressValue As String = If(dr("Address") IsNot DBNull.Value, dr("Address").ToString().ToUpper(), "")
                        Dim miValue As String = If(dr("MI") IsNot DBNull.Value, dr("MI").ToString().ToUpper(), "")
                        Dim suffixValue As String = If(dr("Suffix") IsNot DBNull.Value, dr("Suffix").ToString().ToUpper(), "")

                        lblIDNumber.Text = dr("IDNumber").ToString().ToUpper()
                        lblInfo_LastName.Text = dr("LastName").ToString().ToUpper()
                        lblInfo_FirstName.Text = dr("FirstName").ToString().ToUpper()
                        lblInfo_MI.Text = miValue
                        lblInfo_Suffix.Text = suffixValue
                        lblInfo_Gender.Text = dr("Gender").ToString().ToUpper()
                        lblInfo_BirthDate.Text = Convert.ToDateTime(dr("DateOfBirth")).ToString("MMMM dd, yyyy")
                        lblInfo_Age.Text = dr("Age").ToString()
                        lblInfo_CivilStatus.Text = dr("CivilStatus").ToString().ToUpper()
                        lblInfo_Nationality.Text = dr("Nationality").ToString().ToUpper()
                        lblInfo_Address.Text = addressValue
                        lblInfo_Email.Text = dr("EmailAddress").ToString().ToUpper()
                        lblInfo_Phone.Text = dr("PhoneNumber").ToString()

                        If dr("Photo") IsNot DBNull.Value Then
                            Dim photoBytes As Byte() = CType(dr("Photo"), Byte())
                            Using ms As New MemoryStream(photoBytes)
                                picResident.Image = Image.FromStream(ms)
                                picResident.SizeMode = PictureBoxSizeMode.Zoom
                            End Using
                        Else
                            picResident.Image = Nothing
                        End If

                        ShowLabels()
                    End If
                End Using
            End Using

        Catch ex As Exception
            MessageBox.Show("ERROR: " & ex.Message, "ERROR", MessageBoxButtons.OK, MessageBoxIcon.Error)
        Finally
            DBconnection.CloseConnection()
        End Try
    End Sub

    Private Sub ShowLabels()
        lblIDNumber.Visible = True
        lblInfo_LastName.Visible = True
        lblInfo_FirstName.Visible = True
        lblInfo_MI.Visible = True
        lblInfo_Suffix.Visible = True
        lblInfo_Gender.Visible = True
        lblInfo_BirthDate.Visible = True
        lblInfo_Age.Visible = True
        lblInfo_CivilStatus.Visible = True
        lblInfo_Nationality.Visible = True
        lblInfo_Address.Visible = True
        lblInfo_Email.Visible = True
        lblInfo_Phone.Visible = True
        picResident.Visible = True
    End Sub

    Private Sub btnClose_Click(sender As Object, e As EventArgs) Handles btnClose.Click
        Me.Close()
    End Sub

    Private Sub btnSetAppointment_Click(sender As Object, e As EventArgs) Handles btnSetAppointment.Click
        If cboPurpose.SelectedIndex = -1 Then
            MessageBox.Show("PUMILI NG PURPOSE SA LISTAHAN.", "PAALALA", MessageBoxButtons.OK, MessageBoxIcon.Information)
            Return
        End If

        Try
            DBconnection.connection()

            Dim residentId As Integer = 0
            Using cmdGetId As New MySqlCommand("SELECT id FROM residents WHERE IDNumber = @IDNumber LIMIT 1", DBconnection.cn)
                cmdGetId.Parameters.AddWithValue("@IDNumber", lblIDNumber.Text)
                Dim result = cmdGetId.ExecuteScalar()
                If result IsNot Nothing AndAlso Not IsDBNull(result) Then
                    residentId = Convert.ToInt32(result)
                End If
            End Using

            If residentId = 0 Then
                MessageBox.Show("RESIDENT NOT FOUND!", "ERROR", MessageBoxButtons.OK, MessageBoxIcon.Error)
                Return
            End If

            Dim fullName As String = String.Join(" ", {
                lblInfo_LastName.Text & ",",
                lblInfo_FirstName.Text,
                lblInfo_MI.Text,
                lblInfo_Suffix.Text
            }).Trim().Replace("  ", " ")

            Using cmd As New MySqlCommand("INSERT INTO appointments " &
                "(ResidentID, IDNumber, FullName, FullAddress, EmailAddress, PhoneNumber, AppointmentDate, Purpose, Status) " &
                "VALUES (@ResidentID, @IDNumber, @FullName, @FullAddress, @Email, @Phone, @AppDate, @Purpose, 'PENDING')", DBconnection.cn)

                cmd.Parameters.AddWithValue("@ResidentID", residentId) ' ✅ KAILANGAN ITO!
                cmd.Parameters.AddWithValue("@IDNumber", lblIDNumber.Text)
                cmd.Parameters.AddWithValue("@FullName", fullName.ToUpper())
                cmd.Parameters.AddWithValue("@FullAddress", lblInfo_Address.Text)
                cmd.Parameters.AddWithValue("@Email", lblInfo_Email.Text)
                cmd.Parameters.AddWithValue("@Phone", lblInfo_Phone.Text)
                cmd.Parameters.AddWithValue("@AppDate", DateTime.Now)
                cmd.Parameters.AddWithValue("@Purpose", cboPurpose.SelectedItem.ToString().ToUpper())

                cmd.ExecuteNonQuery()
            End Using

            MessageBox.Show("✅ APPOINTMENT SAVED SUCCESSFULLY!", "SUCCESS", MessageBoxButtons.OK, MessageBoxIcon.Information)
            Me.DialogResult = DialogResult.OK
            Me.Close()

        Catch ex As Exception
            MessageBox.Show("ERROR SAVING APPOINTMENT: " & ex.Message, "ERROR", MessageBoxButtons.OK, MessageBoxIcon.Error)
        Finally
            DBconnection.CloseConnection()
        End Try
    End Sub

End Class