namespace HotelBookingManager;

partial class Form1
{
    /// <summary>
    ///  Required designer variable.
    /// </summary>
    private System.ComponentModel.IContainer components = null;

    //declare a BookingManager Object
    private readonly BookingManager manager = new();

    //Event Sender Method
    private void btnBook_Click(object sender, EventArgs e) => BookRoom();

    private void btnCancel_Click(object sender, EventArgs e) => CancelBooking();
    private void btnView_Click(object sender, EventArgs e) => RefreshList();
    private void btnExit_Click(object sender, EventArgs e) => Close();

    //void to Book a Hotel Room
    private void BookRoom()
    {
        try
        {
            var room = txtRoom.Text.Trim();
            var guest = txtGuest.Text.Trim();
            var c_in = dtIn.Value;
            var c_out = dtOut.Value;

            //Validate that Guest and Room Number are not empty
            if (string.IsNullOrWhiteSpace(room) || string.IsNullOrWhiteSpace(guest))
                throw new ArgumentException("Guest and room are required.");
            //Validate that check out is after check in
            if (c_out <= c_in)
                throw new ArgumentException("Check-Out must be after Check-In.");

            //Create a booking and add it
            var b = new Booking(room, guest, c_in, c_out);
            manager.Add(b);

            //Call helpers
            RefreshList();
            ClearInputs();
            SetStatus($"Room {room} booked for {guest}.", success: true);
        }
        catch (ArgumentException ex)
        {
            MessageBox.Show(ex.Message, "Validation", MessageBoxButtons.OK, MessageBoxIcon.Warning);
            SetStatus(ex.Message, success: false);
        }
        catch (InvalidOperationException ex)
        {
            MessageBox.Show(ex.Message, "Conflict", MessageBoxButtons.OK, MessageBoxIcon.Warning);
            SetStatus(ex.Message, success: false);
        }
        catch (Exception ex)
        {
            MessageBox.Show("Unexpected error. See details.\n", ex.Message,
                MessageBoxButtons.OK, MessageBoxIcon.Error);
            SetStatus("Unexpected Error.", success: false);
        }
    }

    //void to Cancel a Booking
    private void CancelBooking()
    {
        //get information from the text boxes
        var room = txtRoom.Text.Trim();
        var guest = txtGuest.Text.Trim();

        //Reject empty text boxes
        if (string.IsNullOrWhiteSpace(room) || string.IsNullOrWhiteSpace(guest))
        {
            MessageBox.Show("Enter both Room and Guest to cancel.", "Cancel Booking",
                MessageBoxButtons.OK, MessageBoxIcon.Information);
            return;
        }

        //Attempt to Cancel the reservation
        var ok = manager.Cancel(room, guest);
        if (ok)
        {
            RefreshList();
            ClearInputs();
            SetStatus($"Canceled booking for {guest} in room {room}.", success: true);
        }
        else
        {
            MessageBox.Show("No matching booking found.", "Cancel Booking",
                MessageBoxButtons.OK, MessageBoxIcon.Information);
            SetStatus("No matching booking found", success: false);
        }
    }

    //method to refresh the Bookings List View
    private void RefreshList()
    {
        lvBookings.BeginUpdate();
        lvBookings.Items.Clear();

        foreach (var b in manager.All())
        {
            var item = new ListViewItem(new[]
                {
                     b.RoomNumber,
                     b.CheckIn.ToString("yyyy-MM-dd HH:mm"),
                     b.CheckOut.ToString("yyyy-MM-dd HH:mm"),
                     b.GuestName,
            });
            lvBookings.Items.Add(item);
        }

        lvBookings.AutoResizeColumns(ColumnHeaderAutoResizeStyle.ColumnContent);
        lvBookings.EndUpdate();
        SetStatus($"Loaded {lvBookings.Items.Count} booking(s).", success: true);
    }

    //void to load selected booking into inputs
    private void LoadSeclectionIntoInputs()
    {
        if (lvBookings.SelectedItems.Count == 0) return;
        var sel = lvBookings.SelectedItems[0];
        txtRoom.Text = sel.SubItems[0].Text;
        dtIn.Value = DateTime.Parse(sel.SubItems[1].Text);
        dtOut.Value = DateTime.Parse(sel.SubItems[2].Text);
        txtGuest.Text = sel.SubItems[3].Text;
        SetStatus("Loaded selection into inputs.", success: true);
    }

    //void to update the Status Label
    private void SetStatus(string message, bool success)
    {
        lblStatus.Text = message;
        lblStatus.ForeColor = success ? Color.FromArgb(102, 235, 130) : Color.FromArgb(255, 179, 162);
    }

    //void to clear all inputs
    private void ClearInputs()
    {
        txtGuest.Clear();
        txtRoom.Clear();
        txtGuest.Focus();
    }

    /// <summary>
    ///  Clean up any resources being used.
    /// </summary>
    /// <param name="disposing">true if managed resources should be disposed; otherwise, false.</param>
    protected override void Dispose(bool disposing)
    {
        if (disposing && (components != null))
        {
            components.Dispose();
        }
        base.Dispose(disposing);
    }

    #region Windows Form Designer generated code

    /// <summary>
    ///  Required method for Designer support - do not modify
    ///  the contents of this method with the code editor.
    /// </summary>
    private void InitializeComponent()
    {
        lblWelcomeLabel = new Label();
        lblGuestName = new Label();
        lblRoomNumber = new Label();
        lblCheckIn = new Label();
        lblCheckOut = new Label();
        txtGuest = new TextBox();
        txtRoom = new TextBox();
        dtIn = new DateTimePicker();
        dtOut = new DateTimePicker();
        btnBook = new Button();
        btnCancel = new Button();
        btnView = new Button();
        btnExit = new Button();
        lblStatus = new Label();
        lvBookings = new ListView();
        Room = new ColumnHeader();
        Check_In = new ColumnHeader();
        Check_Out = new ColumnHeader();
        Guest_Name = new ColumnHeader();
        SuspendLayout();
        // 
        // lblWelcomeLabel
        // 
        lblWelcomeLabel.AutoSize = true;
        lblWelcomeLabel.Font = new Font("Modern No. 20", 19.7999973F, FontStyle.Bold, GraphicsUnit.Point, 0);
        lblWelcomeLabel.ForeColor = SystemColors.ButtonFace;
        lblWelcomeLabel.Location = new Point(248, 32);
        lblWelcomeLabel.Name = "lblWelcomeLabel";
        lblWelcomeLabel.Size = new Size(454, 34);
        lblWelcomeLabel.TabIndex = 0;
        lblWelcomeLabel.Text = "Welcome to the Magneto Hotel";
        lblWelcomeLabel.Click += lblWelcomeLabel_Click;
        // 
        // lblGuestName
        // 
        lblGuestName.AutoSize = true;
        lblGuestName.Font = new Font("Segoe UI", 10.2F, FontStyle.Regular, GraphicsUnit.Point, 0);
        lblGuestName.ForeColor = SystemColors.ControlLight;
        lblGuestName.Location = new Point(80, 111);
        lblGuestName.Name = "lblGuestName";
        lblGuestName.Size = new Size(109, 23);
        lblGuestName.TabIndex = 1;
        lblGuestName.Text = "Guest Name:";
        lblGuestName.TextAlign = ContentAlignment.TopRight;
        // 
        // lblRoomNumber
        // 
        lblRoomNumber.AutoSize = true;
        lblRoomNumber.Font = new Font("Segoe UI", 10.2F, FontStyle.Regular, GraphicsUnit.Point, 0);
        lblRoomNumber.ForeColor = SystemColors.ControlLight;
        lblRoomNumber.Location = new Point(530, 115);
        lblRoomNumber.Name = "lblRoomNumber";
        lblRoomNumber.Size = new Size(127, 23);
        lblRoomNumber.TabIndex = 2;
        lblRoomNumber.Text = "Room Number:";
        lblRoomNumber.TextAlign = ContentAlignment.TopRight;
        // 
        // lblCheckIn
        // 
        lblCheckIn.AutoSize = true;
        lblCheckIn.Font = new Font("Segoe UI", 10.2F, FontStyle.Regular, GraphicsUnit.Point, 0);
        lblCheckIn.ForeColor = SystemColors.ControlLight;
        lblCheckIn.Location = new Point(80, 205);
        lblCheckIn.Name = "lblCheckIn";
        lblCheckIn.Size = new Size(82, 23);
        lblCheckIn.TabIndex = 4;
        lblCheckIn.Text = "Check-In:";
        lblCheckIn.TextAlign = ContentAlignment.TopRight;
        // 
        // lblCheckOut
        // 
        lblCheckOut.AutoSize = true;
        lblCheckOut.Font = new Font("Segoe UI", 10.2F, FontStyle.Regular, GraphicsUnit.Point, 0);
        lblCheckOut.ForeColor = SystemColors.ControlLight;
        lblCheckOut.Location = new Point(522, 205);
        lblCheckOut.Name = "lblCheckOut";
        lblCheckOut.Size = new Size(96, 23);
        lblCheckOut.TabIndex = 5;
        lblCheckOut.Text = "Check-Out:";
        lblCheckOut.TextAlign = ContentAlignment.TopRight;
        // 
        // txtGuest
        // 
        txtGuest.Location = new Point(195, 110);
        txtGuest.MaxLength = 250;
        txtGuest.Name = "txtGuest";
        txtGuest.Size = new Size(307, 27);
        txtGuest.TabIndex = 6;
        txtGuest.Text = "Guest Name";
        // 
        // txtRoom
        // 
        txtRoom.Location = new Point(663, 114);
        txtRoom.Name = "txtRoom";
        txtRoom.Size = new Size(112, 27);
        txtRoom.TabIndex = 7;
        txtRoom.Text = "Room (e.g. 101)";
        // 
        // dtIn
        // 
        dtIn.Location = new Point(168, 202);
        dtIn.Name = "dtIn";
        dtIn.Size = new Size(250, 27);
        dtIn.TabIndex = 8;
        // 
        // dtOut
        // 
        dtOut.Location = new Point(624, 204);
        dtOut.Name = "dtOut";
        dtOut.Size = new Size(250, 27);
        dtOut.TabIndex = 9;
        dtOut.ValueChanged += dtOut_ValueChanged;
        // 
        // btnBook
        // 
        btnBook.Location = new Point(164, 282);
        btnBook.Name = "btnBook";
        btnBook.Size = new Size(132, 29);
        btnBook.TabIndex = 10;
        btnBook.Text = "Book Room";
        btnBook.UseVisualStyleBackColor = true;
        btnBook.Click += btnBook_Click;
        // 
        // btnCancel
        // 
        btnCancel.Location = new Point(334, 282);
        btnCancel.Name = "btnCancel";
        btnCancel.Size = new Size(132, 29);
        btnCancel.TabIndex = 11;
        btnCancel.Text = "Cancel Booking";
        btnCancel.UseVisualStyleBackColor = true;
        btnCancel.Click += btnCancel_Click;
        // 
        // btnView
        // 
        btnView.Location = new Point(508, 282);
        btnView.Name = "btnView";
        btnView.Size = new Size(139, 29);
        btnView.TabIndex = 12;
        btnView.Text = "View All Bookings";
        btnView.UseVisualStyleBackColor = true;
        btnView.Click += btnView_Click;
        // 
        // btnExit
        // 
        btnExit.Location = new Point(681, 282);
        btnExit.Name = "btnExit";
        btnExit.Size = new Size(94, 29);
        btnExit.TabIndex = 13;
        btnExit.Text = "Exit";
        btnExit.UseVisualStyleBackColor = true;
        btnExit.Click += btnExit_Click;
        // 
        // lblStatus
        // 
        lblStatus.AutoSize = true;
        lblStatus.Font = new Font("Segoe UI", 12F, FontStyle.Regular, GraphicsUnit.Point, 0);
        lblStatus.ForeColor = SystemColors.ButtonHighlight;
        lblStatus.Location = new Point(80, 559);
        lblStatus.Name = "lblStatus";
        lblStatus.Size = new Size(69, 28);
        lblStatus.TabIndex = 14;
        lblStatus.Text = "Ready.";
        // 
        // lvBookings
        // 
        lvBookings.Columns.AddRange(new ColumnHeader[] { Room, Check_In, Check_Out, Guest_Name });
        lvBookings.Location = new Point(80, 326);
        lvBookings.Name = "lvBookings";
        lvBookings.Size = new Size(819, 217);
        lvBookings.TabIndex = 15;
        lvBookings.UseCompatibleStateImageBehavior = false;
        lvBookings.View = View.Details;
        lvBookings.SelectedIndexChanged += lvBookings_SelectedIndexChanged;
        // 
        // Room
        // 
        Room.Text = "Room";
        Room.Width = 80;
        // 
        // Check_In
        // 
        Check_In.Text = "Check-In";
        Check_In.Width = 140;
        // 
        // Check_Out
        // 
        Check_Out.Text = "Check-Out";
        Check_Out.Width = 140;
        // 
        // Guest_Name
        // 
        Guest_Name.Text = "Guest";
        Guest_Name.Width = 250;
        // 
        // Form1
        // 
        AutoScaleDimensions = new SizeF(8F, 20F);
        AutoScaleMode = AutoScaleMode.Font;
        BackColor = Color.Indigo;
        ClientSize = new Size(965, 610);
        Controls.Add(lvBookings);
        Controls.Add(lblStatus);
        Controls.Add(btnExit);
        Controls.Add(btnView);
        Controls.Add(btnCancel);
        Controls.Add(btnBook);
        Controls.Add(dtOut);
        Controls.Add(dtIn);
        Controls.Add(txtRoom);
        Controls.Add(txtGuest);
        Controls.Add(lblCheckOut);
        Controls.Add(lblCheckIn);
        Controls.Add(lblRoomNumber);
        Controls.Add(lblGuestName);
        Controls.Add(lblWelcomeLabel);
        Name = "Form1";
        Text = "Hotel Booking Manager";
        ResumeLayout(false);
        PerformLayout();
    }

    #endregion

    private Label lblWelcomeLabel;
    private Label lblGuestName;
    private Label lblRoomNumber;
    private Label lblCheckIn;
    private Label lblCheckOut;
    private TextBox txtGuest;
    private TextBox txtRoom;
    private DateTimePicker dtIn;
    private DateTimePicker dtOut;
    private Button btnBook;
    private Button btnCancel;
    private Button btnView;
    private Button btnExit;
    private Label lblStatus;
    private ListView lvBookings;
    private ColumnHeader Room;
    private ColumnHeader Check_In;
    private ColumnHeader Check_Out;
    private ColumnHeader Guest_Name;
}
