using System;
using System.Collections.Generic;
using System.Text;

namespace DesignPatterns.Command.Example1
{
    public class PlayWithRemote
    {
        public static void Run()
        {
            // Gets the ElectronicDevice to use         
            IElectronicDevice newDevice = TVRemote.GetDevice();

            // TurnTVOn contains the command to turn on the tv
            // When execute() is called on this command object
            // it will execute the method on() in Television
            TurnTVOn onCommand = new TurnTVOn(newDevice);

            // Calling the execute() causes on() to execute in Television
            DeviceButton button = new DeviceButton(onCommand);

            // When press() is called theCommand.execute(); executes
            button.Press();

            //----------------------------------------------------------

            // Now when execute() is called off() of Television executes
            TurnTVOff offCommand = new TurnTVOff(newDevice);

            // Calling the execute() causes off() to execute in Television
            button = new DeviceButton(offCommand);

            // When press() is called theCommand.execute(); executes
            button.Press();

            //----------------------------------------------------------

            // Now when execute() is called volumeUp() of Television executes
            TurnTVUp volUpCommand = new TurnTVUp(newDevice);

            // Calling the execute() causes volumeUp() to execute in Television
            button = new DeviceButton(volUpCommand);

            // When press() is called theCommand.execute(); executes
            button.Press();
            button.Press();
            button.Press();


            //----------------------------------------------------------
            // Creating a TV and Radio to turn off with 1 press
            Television theTV = new Television();
            Radio theRadio = new Radio();

            // Add the Electronic Devices to a List
            List<IElectronicDevice> devices = new List<IElectronicDevice>
            { theTV, theRadio };

            // Send the List of Electronic Devices to TurnItAllOff
            // where a call to run execute() on this function will
            // call off() for each device in the list


            TurnItAllOff turnOffDevices = new TurnItAllOff(devices);

            DeviceButton turnOffAllButton = new DeviceButton(turnOffDevices);

            turnOffAllButton.Press();

            //----------------------------------------------------------
            /*    
             * It is common to be able to undo a command in a command pattern    
             * To do so, DeviceButton will have a method called undo    
             * Undo() will perform the opposite action that the normal    
             * Command performs. undo() needs to be added to every class    
             * with an execute()    
             */

            turnOffAllButton.PressUndo();

            // To undo more than one command add them to a LinkedList using addFirst().
            // Then execute undo on each item until there are none left.
            // (This is your Homework)
            
            /* Colocaria um setCommand no invoker para não precisar ficar recriando o objeto
             * Adicionaria na LinkedList toda vez que o botão fosse pressionado.
             * Dessa forma resolveria o desafio deixado no exemplo
             */

        }

    }
}
