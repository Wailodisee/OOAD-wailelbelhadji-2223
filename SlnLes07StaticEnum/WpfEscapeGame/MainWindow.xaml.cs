using System.Windows;
using System.Windows.Controls;
using System.Windows.Documents;

namespace WpfEscapeGame
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        Room currentRoom;
        public MainWindow()
        {
            InitializeComponent();

            // define room
            Room room1 = new Room("bedroom", "I seem to be in a medium sized bedroom. There is a locker to the left, a nice rug on the floor, and a bed to the right. ");

            // define items
            Item key1 = new Item("small silver key", "A small silver key, makes me think of one I at highschool. ", true);
            Item key2 = new Item("large key", "A large key. Could this be my way out? ", true);
            Item Chair = new Item("Chair", "This is a golden chair with this chair nothing is impossible", false);
            Item Poster = new Item("Poster", "With this poster you can read the futur", true);
            Item locker = new Item("locker", "A locker. I wonder what's inside. ", false);
            locker.HiddenItem = key2;
            locker.IsLocked = true;
            locker.Key = key1;
            Item bed = new Item("bed", "Just a bed. I am not tired right now. ", false);
            bed.HiddenItem = key1;

            // setup bedroom
            room1.Items.Add(new Item("floor mat", "A bit ragged floor mat, but still one of the most popular designs. "));
            room1.Items.Add(bed);
            room1.Items.Add(locker);
            room1.Items.Add(Chair);
            room1.Items.Add(Poster);

            // start game
            currentRoom = room1;
            lblMessage.Content = "I am awake, but cannot remember who I am!? Must have been a hell of a party last night... ";
            txtRoomDesc.Text = currentRoom.Description;
            UpdateUI();

            // define room listbox with a room 
            lstRoomDoors.Items.Add("green door");

            // Define Living Room
            Room livingRoom = new Room("living room", "A cozy living room with a couch, a TV, and a bookshelf.");
            Item couch = new Item("couch", "A comfortable couch to relax on.", false);
            Item tv = new Item("TV", "A large flat-screen TV.", false);
            Item bookshelf = new Item("bookshelf", "A wooden bookshelf filled with various books.", false);
            livingRoom.Items.Add(couch);
            livingRoom.Items.Add(tv);
            livingRoom.Items.Add(bookshelf);

            // Define Computer Room
            Room computerRoom = new Room("computer room", "A small room with a computer desk, a chair, and some shelves.");
            Item computer = new Item("computer", "A powerful desktop computer.", false);
            Item desk = new Item("desk", "A simple computer desk.", false);
            Item officeChair = new Item("office chair", "A comfortable office chair.", false);
            computerRoom.Items.Add(computer);
            computerRoom.Items.Add(desk);
            computerRoom.Items.Add(officeChair);

            // Define Doors
            Door door1 = new Door("green door", "A locked green door leading to the living room.", livingRoom, false, key2);
            Door door2 = new Door("white door", "An open white door connecting the living room and computer room.", computerRoom, true, null);
            Door door3 = new Door("blue door", "An open blue door connecting the computer room and living room.", livingRoom, true, null);
            Door door4 = new Door("red door", "A locked red door with no known destination.", null, false, null);

            // Add doors to the current room (bedroom)
            room1.Items.Add(door1);
            livingRoom.Items.Add(door2);
            livingRoom.Items.Add(door4);
            computerRoom.Items.Add(door3);

        }
        private void UpdateUI()
        {
            lstRoomItems.Items.Clear();
            lstRoomDoors.Items.Clear();
            foreach (Item itm in currentRoom.Items)
            {
                if (itm is Door)
                {
                    lstRoomDoors.Items.Add(itm);
                }
                else
                {
                    lstRoomItems.Items.Add(itm);
                }
            }
        }

        private void lstRoomItems_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            btnCheck.IsEnabled = lstRoomItems.SelectedValue != null; // room item selected
            btnPickUp.IsEnabled = lstRoomItems.SelectedValue != null; // room item selected
            btnUseOn.IsEnabled = lstRoomItems.SelectedValue != null && lstMyItems.SelectedValue != null; // room itemand picked up item selected

        }

        private void btnCheck_Click(object sender, RoutedEventArgs e)
        {
            // 1. find item to check
            Item roomItem = (Item)lstRoomItems.SelectedItem;
            // 2. is it locked?
            if (roomItem.IsLocked)
            {
                lblMessage.Content = $"{roomItem.Description}It is firmly locked. ";
                return;
            }
            // 3. does it contain a hidden item?
            Item foundItem = roomItem.HiddenItem;
            if (foundItem != null)
            {
                lblMessage.Content = $"Oh, look, I found a {foundItem.Name}. ";
                lstMyItems.Items.Add(foundItem);
                roomItem.HiddenItem = null;
                return;
            }
            // 4. just another item; show description
            lblMessage.Content = roomItem.Description;

        }

        private void btnPickUp_Click(object sender, RoutedEventArgs e)
        {
            // 1. find selected item
            Item selItem = (Item)lstRoomItems.SelectedItem;
            if (!selItem.IsPortable)
            {
                lblMessage.Content = "You can't take this Item.";
                return;
            }
            // 2. add item to your items list
            lblMessage.Content = $"I just picked up the {selItem.Name}. ";
            lstMyItems.Items.Add(selItem);
            lstRoomItems.Items.Remove(selItem);
            currentRoom.Items.Remove(selItem);


        }

        private void btnUseOn_Click(object sender, RoutedEventArgs e)
        {
            // 1. find both items
            Item myItem = (Item)lstMyItems.SelectedItem;
            Item roomItem = (Item)lstRoomItems.SelectedItem;
            // 2. item doesn't fit
            if (roomItem.Key != myItem)
            {
                switch (roomItem.ToString())
                {
                    case "locker": lblMessage.Content = RandomMessageGenerator.GetRandomMessage(MessageType.Keys);break;
                    case "Chair": lblMessage.Content = RandomMessageGenerator.GetRandomMessage(MessageType.DoubleItem);break;
                    case "Poster": lblMessage.Content = RandomMessageGenerator.GetRandomMessage(MessageType.DoubleItem);break;
                    case "Bed": lblMessage.Content = RandomMessageGenerator.GetRandomMessage(MessageType.DoubleItem);break;
                    default: lblMessage.Content = RandomMessageGenerator.GetRandomMessage(MessageType.NoItem);break;
                }
                return;
            }
            // 3. item fits; other item unlocked
            roomItem.IsLocked = false;
            roomItem.Key = null;
            lstMyItems.Items.Remove(myItem);
            lblMessage.Content = $"I just unlocked the {roomItem.Name}!";

        }

        private void lstMyItems_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            btnCheck.IsEnabled = lstRoomItems.SelectedValue != null; // room item selected
            btnPickUp.IsEnabled = lstRoomItems.SelectedValue != null; // room item selected
            btnUseOn.IsEnabled = lstRoomItems.SelectedValue != null && lstMyItems.SelectedValue != null; // room itemand picked up item selected
            btnDrop.IsEnabled = lstMyItems.SelectedValue != null;

        }

        private void btnDrop_Click(object sender, RoutedEventArgs e)
        {
            if (lstMyItems.SelectedItem != null)
            {
                Item DropItem = (Item)lstMyItems.SelectedItem;
                string SelectedItem = lstMyItems.SelectedItem.ToString();
                lstMyItems.Items.Remove(DropItem);
                lstRoomItems.Items.Add(SelectedItem);
            }
        }

        private void btnOpenwith_Click(object sender, RoutedEventArgs e)
        {
            Item selectedItem = (Item)lstRoomItems.SelectedItem;
            if (selectedItem == null)
            {
                MessageBox.Show("Select an item from your inventory to use.");
                return;
            }

            if (selectedItem.Key == null)
            {
                MessageBox.Show($"{selectedItem.Name} cannot be used to open anything.");
                return;
            }

            Item selectedDoor = (Item)lstRoomDoors.SelectedItem;
            if (selectedDoor == null)
            {
                MessageBox.Show("Select a door to open.");
                return;
            }

            if (!selectedDoor.GetType().Equals(typeof(Door)))
            {
                MessageBox.Show("Selected item is not a door.");
                return;
            }

            Door door = (Door)selectedDoor;
            door.Open(selectedItem.Key);
            UpdateUI();

        }

        private void btnEnter_Click(object sender, RoutedEventArgs e)
        {
            // Krijg de geselecteerde deur
            Door selectedDoor = (Door)lstRoomDoors.SelectedItem;

            // Als de deur niet open is, stop de methode
            if (selectedDoor == null || !selectedDoor.IsOpen)
            {
                return;
            }
            // Ga naar de nieuwe kamer
            currentRoom = selectedDoor.DestinationRoom;
            UpdateUI();
        }
    }
}
