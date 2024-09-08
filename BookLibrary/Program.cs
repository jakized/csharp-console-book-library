
using BookLibrary;

DatabaseHelper db = new DatabaseHelper();
LibraryController lc = new LibraryController(db);
UserInterface ui = new UserInterface(lc);

ui.MainMenu();