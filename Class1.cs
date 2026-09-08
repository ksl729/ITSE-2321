using System;
using System.Collections.Generic;
using System.Drawing;
using System.Text;

namespace Week3Lecture;

// Create the class 'Sandwich'
internal class Sandwich
{

    string breadType;
    int numberOfSlices;
    string protein; //Type of protein
    int numberOfProtein; // Number of slices of protein
    bool hasCheese;
    string condiment;
    bool isToasted;
    string size;
    string fruit;
    string vegetable;
    bool hasDecoration;


    // This is the conctructor
    // It is a special method that is called when an object of the class is created
    // Ex. Sandwich mySandwich = new Sandwich("Turkey Sandwich", "Wheat", "Large");
    public Sandwich(string SandwichName, string BreadType, string Size)
    {// Signature of the constructor is NOT the same as the class name
        sandwichName = SandwichName;
        breadType = BreadType;
        numberOfSlices = 2;
        protein = "N/A";
        numberOfProtein = 0;
        hasCheese = false;
        condiment = "N/A";
        isToasted = false;
        size = Size;
        fruit = "N/A";
        vegetable = "N/A";
        hasDecoration = false;
    }

    // This is the conctructor
    // It is a special method that is called when an object of the class is created
    public Sandwich(string sandwichName, string breadType, string size, string protein, int numberOfProtein, bool hasCheese, string condiment, bool isToasted, string fruit, string vegetable, bool hasDecoration)
    {// Signature of the constructor has more of the attributes listed in the class
        this.sandwichName = sandwichName;
        this.breadType = breadType;
        this.numberOfSlices = 2;
        this.protein = "N/A";
        this.numberOfProtein = 0;
        this.hasCheese = false;
        this.condiment = "N/A";
        this.isToasted = false;
        this.size = size;
        this.fruit = "N/A";
        this.vegetable = "N/A";
        this.hasDecoration = false;
    }

    // This is a method that will return the value of the attribute sandwichName
    public string GetName()
    {
        return sandwichName;
    }

    // The void means this will not return anything, it will just set the value of the attribute
    public void SetName(string newValue)
    {
        sandwichName = newValue;
    }

    public void SetCheese(bool newValue)
    {
        hasCheese = newValue;
    }   

    // Prints out all available attributes of the sandwich, even if they are not set
    public override string ToString()
    {
        return "Sandwich Name: " + sandwichName +
               "\nBread Type: " + breadType +
               "\nNumber of Slices: " + numberOfSlices +
               "\nType of Protein: " + protein +
               "\nNumber of Protein Slices: " + numberOfProtein +
               "\nWith Cheese?: " + hasCheese;
               
               ;

    }
}