#Disastroid Controller

##Introduction
The Disastroid controller is meant to be a library that allows a developer to use an Android phones abilities to become a game controller with innovative and custom interactions.

Disastroid-Unity is the Untity side of the library and is meant to handle the custom interactions messages send by the phone/game controller and made these messages available to the through a class similar to the InputManager from Unity.

##Usage
###Importing our InputManager
Just add the file `NetworkInputManager.cs` to your Scene in Unity and the file `UDPPacketIO.cs` to your script folder.
###Define your own controller
Disastroid controller is meant to be extended by your own controller implementation just by extending the class `Controller`

We provide an example of implementation of our own controller with the `DisastroidController`.

To add or overwrite customs actions to your controller implementation you have to add new values to `Controller.actions` which is a `Dictionary<string, CommandReceivedDelegate>`. The string key has to correspond to the command identifier sent by the Android application ("Move" or "Orientation" are some default possible values) whereas the `CommandReceivedDelegate` is the delegate declared as `delegate void CommandReceivedDelegate(float x, float y, float z)`.

This delegate will be called each time the corresponding `string` command is received and should use values x, y and z to modify the state of the `Controller` instance.

**Example :**

    public bool FireCommandRegistered = false;
    public void CommandFire(float x, float y, float z)
    {
        FireCommandRegistered = true;
    }


###Detecting the addition of a new controller
Often times we want to be able to perform special operation when a new controller is detected (eg: add a new player sprite to the game scene,...).

To handle that, at the creation of the scene, you can set a delegate method to the `NetworkInputManager` : 

`public delegate void NewControllerDelegate(string controllerIP);`

This delegate will be called each time a new controller is communicating with the game, `controllerIP` containing the IP address corresponding to this new controller.

In this delegate you should at least instanciate a new instance of `Controller` and add it to `NetworkInputManager`.

**Example :**

    public void addNewPlayer(string controllerIP)
    {
          NetworkInputManager.ConnectedControllers[controllerIP] = new DisastroidController();
    }

###Reading the controller state
In a classic Unity project you check the state of the controller like this :


    bool shoot = Input.GetButtonDown("Fire1");

With our NetworkInputManager, because we manage several controllers, it looks like this :

    bool shoot = NetworkInputManager.ConnectedControllers[controllerIP].FireCommandRegistered;
