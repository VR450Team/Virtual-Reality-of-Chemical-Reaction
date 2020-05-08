As you know, when creating a project in Unity, many files that are not directly useful to the user are created. For ease of grading purposed this file will have 2 parts. First, use of the application will be described and then there will be a section describing the file structure to assist during the grading process. 

Our project was created using Unity 2018.4.19f1(64-bit).

***Application Use***

The unity project is located in a folder titled, “Virtual-Reality-of-Chemical-Reaction.”
Open this folder and then double click the file titled, “Virtual Reality of Chemical Reactions.exe”.
This will open up the “Virtual Reality of Chemical Reactions Configuration” window where you can select the ‘Play!’ button. This will launch the application in full screen but if you would like the application to be a window so you can also view this documentation, you can check the ‘Windowed’ box before selecting ‘Play’. This will launch the application and pull up the main menu.

- Main Menu
The main menu displays 4 buttons:
‘FILE SELECTION’ - navigates to the file selection scene.
‘DOWNLOAD FROM SERVER’ – navigates to the download from server scene.
‘EXIT’ – exits the application.
‘HELP’ – displays the various functions binded to keyboard inputs. There is a button to return to the main menu.

- Download From Server Scene
The file selection scene displays an input text box and two buttons.
The input textbox allows the user to specify which files they would like to pull from the web server, one at a time.
Once the user has typed the file name into the textbox, they can click the request file button. 
Our current version of the project has two reaction files. We have stored both files on the server and directly in the project by different names to show functionality of the web server. The files on the server are named ‘officialReaction1.txt’ and ‘officialReaction2.txt’. These names can individually be typed into the text box and then ‘Request File’ can be selected. The user will be notified if pulling the file from the server was successful or if it failed in the shaded box. 
Once the user has downloaded the files they would like, they can go back to the main menu and click ‘FILE SELECTION’.

- File Selection Scene
The box in the middle is populated with buttons labeled with file/reaction options for the user to choose from, including both the locally stored files and the files the user downloaded from the web server. 'officialReaction1' and 'officialReaction2' are from the web server and 'reaction1' and 'reaction2' are stored in the files of the project. Once the user has selected a reaction, the selected reaction will be displayed in the shaded box on the right side. From here, the use can use the ‘Launch’ button to launch the selected reaction.
At any point while viewing the file selection scene, the user can hit the ‘Main Menu’ button to return to the main menu.

- Simulation Scene
The user will see the selected reaction as well as a navigation menu visible on screen. On the navigation menu the user will have the following functionality tied to the listed button or keyboard inputs:

Zoom the camera view in:
	Button: ’+’
	Key: ‘i’
Zoom the camera view out:
	Button: ’-’
	Key: ‘o’
Rotate the camera view up:
	Button: ‘˄’
	Key: ‘↑’
Rotate the camera view down:
	Button: ’ ˅’
	Key: ‘↓’
Rotate the camera view to the right:
	Button: ’ ˃’
	Key: ‘→’
Rotate the camera view to the left:
	Button: ’˂’
	Key: ‘←’
Pause/Play:
	Button: /
	Key: spacebar
Restart the current reaction:
  	Button: ’Restart’
	Key: ‘r’
Return to File Selection:
  	Button: ’New Reaction’
	Key: ‘f’
Return to Main Menu:
  	Button: ’Main Menu’
	Key: ‘e’
	
The user can return to the main menu and exit the application at any time. 

***File Structure***
Our unity project is stored in the folder titled, “Virtual-Reality-of-Chemical-Reaction”. The files listed below will include the files we created or edited, any unnamed files were created by Unity and not directly modified by our team.

The relevant folders include:
	‘Assets’
The relevant files include:
	‘Virtual Reality of Chemical Reactions.exe
	‘atom colors.png’

Within the ‘Assets’ folder, the relevant files are:
	‘Files’ – The two reaction files in our project are stored here under the names of ‘reaction1.txt’ and reaction2.txt.
	‘Prefabs’ – This file contains the prefabs used to instantiate the atoms and covalent bonds in a simulation as well as a button prefab. Prefabs are best viewed in unity editor. The hexadecimal values match the values provided by Dr. Seibert in the ‘atom colors.png’ mentioned above. The prefabs use information from the ‘Materials’ folder such as color but that folder is not important. 
	‘Scenes’ – This folder includes all of the scenes that are present in our project such as:
		‘Download.unity’ – the scene to allow the users to pull files from a web server.
		‘FileSelect.unity’ – the scene to allow the users to select a file.
		‘Help.unity’ – the scene to display the keys binded to navigation menu functions.
		‘MainMenu.unity’ – the main menu scene.
		‘MainScene.unity’ – the scene where the simulations are displayed.
	‘Scripts’ – All of our C# scripts are stored here.
		‘AtomScript.cs’
		‘BondScript.cs’
		‘DownloadSceneButtonActions.cs’
		‘FileSelectButtonCreator.cs’
		‘LaunchButtonScript.cs’
		‘MainMenu.cs’
		‘NavMenuButtons.cs’
		‘ReturnMain.cs’ 
