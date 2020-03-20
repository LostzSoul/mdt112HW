using System;
using System.Collections.Generic;
using System.Diagnostics;

namespace mdt112_assignment2
{
    public enum Size{
        SMALL, MEDIUM, LARGE
    }

    interface IDisassemblable{
        float GetDisassembleCost();
        List<MaterialGarbage> Disassemble();
    }

    interface ISellable{
        float GetSellPrice();
    }

    class Garbage{
        public string Label;
        protected Size Size;

        public Size GetSize(){
            return Size;
        }

        public Garbage( string label, Size size ){
            Label = label;
            Size = size;
        }
    }

    class MaterialGarbage : Garbage, ISellable{
        private float Price;

        public MaterialGarbage( string label, Size size, float price ) : base( label, size ){
            Price = price;
        }

        public float GetSellPrice(){
            switch( Size ){
                case Size.MEDIUM:
                    return 1.5f * Price;
                case Size.LARGE:
                    return 2.5f * Price;
                default:
                    return Price;
            }
        }
    }

    class ElectronicsGarbage : Garbage, ISellable, IDisassemblable{
        private float Price;

        public ElectronicsGarbage( string label, Size size, float price ) : base( label, size ){
            Price = price;
        }

        public float GetSellPrice(){
            switch( Size ){
                case Size.MEDIUM:
                    return 1.4f * Price;
                case Size.LARGE:
                    return 1.6f * Price;
                default:
                    return 1.2f * Price;
            }
        }

        public float GetDisassembleCost(){
            switch( Size ){
                case Size.MEDIUM:
                    return 0.4f * Price;
                case Size.LARGE:
                    return 0.5f * Price;
                default:
                    return 0.3f * Price;
            }
        }

        public List<MaterialGarbage> Disassemble(){
            List<MaterialGarbage> disassembledGarbageList = new List<MaterialGarbage>();

            switch(Size){
                case Size.MEDIUM:
                    disassembledGarbageList.Add( new MaterialGarbage( Label+"_metal", Size.MEDIUM, 210.5f ) );
                    disassembledGarbageList.Add( new MaterialGarbage( Label+"_glass", Size.SMALL, 100.0f ) );
                    break;
                case Size.LARGE:
                    disassembledGarbageList.Add( new MaterialGarbage( Label+"_plastic", Size.MEDIUM, 80.0f ) );
                    disassembledGarbageList.Add( new MaterialGarbage( Label+"_metal", Size.LARGE, 230.5f ) );
                    disassembledGarbageList.Add( new MaterialGarbage( Label+"_glass", Size.LARGE, 120.15f ) );
                    break;
                default:
                    disassembledGarbageList.Add( new MaterialGarbage( Label+"_metal", Size.SMALL, 205.5f ) );
                    break;
            }

            return disassembledGarbageList;
        }
    }

    class GlassBottleGarbage : Garbage, ISellable{
        private float Price;

        public GlassBottleGarbage( string label, float price ) : base( label, Size.MEDIUM ){
            Price = price;
        }

        public float GetSellPrice(){
            return Price;
        }
    }

    class Quiz{
        ///<summary>
        ///This function sort given garbage list by its size ascendingly 
        ///from small, medium to large with an option to sort reversely.
        ///</summary>
        public static List<Garbage> SortGarbageListBySize( List<Garbage> garbageList, bool isReverse ){
            List<Garbage> ReGarbage = new List<Garbage>();

            for (int i = 0; i < garbageList.Count; i++)
            {
                Size x = garbageList[i].GetSize();

                if (x == Size.SMALL)
                {
                    ReGarbage.Add(garbageList[i]);
                }
            }
            for (int j = 0; j < garbageList.Count; j++)
            {
                Size x = garbageList[j].GetSize();

                if (x == Size.MEDIUM)
                {
                    ReGarbage.Add(garbageList[j]);
                }
            }
            for (int k = 0; k < garbageList.Count; k++)
            {
                Size x = garbageList[k].GetSize();

                if (x == Size.LARGE)
                {
                    ReGarbage.Add(garbageList[k]);
                }
            }

            if (isReverse == true)
            {

                ReGarbage.Reverse();
            }
            return ReGarbage;
        }

        ///<summary>
        ///This function filter out only sellable garbage from given
        ///garbage list.
        ///</summary>
        public static List<Garbage> GetSellableGarbageList( List<Garbage> garbageList ){
            //  NOTE: Read more on this link https://docs.microsoft.com/en-us/dotnet/csharp/language-reference/keywords/is
            List<Garbage> SellGarbage = new List<Garbage>();
            for (int i = 0; i < garbageList.Count; i++)
            {
                if (garbageList[i] is ISellable)
                {
                    SellGarbage.Add(garbageList[i]);
                }
            }


            return SellGarbage;
        }

        ///<summary>
        ///This function computes maximum possible sell price from
        ///garbage list. Some garbage may be disassembled into
        ///material garbages that are sellable, yet there is a
        ///disassemblement cost to pay.
        ///</summary>
        public static float ComputeMaximumSellPrice( List<Garbage> garbageList ){
            //  NOTE: Read more on this link https://docs.microsoft.com/en-us/dotnet/csharp/programming-guide/types/casting-and-type-conversions
            List<MaterialGarbage> outputgarbageList = new List<MaterialGarbage>();
            List<float> outputgarbageListPrice = new List<float>();
            for (int i = 0; i < garbageList.Count; i++)
            {
                if (garbageList[i] is ISellable)
                {
                    if (garbageList[i] is IDisassemblable)
                    {
                        ElectronicsGarbage output = (ElectronicsGarbage)garbageList[i];
                        outputgarbageList = output.Disassemble();
                        float DisassemblePrice = 0.0f;
                        float price = 0.0f;
                        for (int j = 0; j < outputgarbageList.Count; j++)
                        {
                            price = outputgarbageList[j].GetSellPrice();
                            DisassemblePrice = DisassemblePrice + price;
                        }
                        Size a = output.GetSize();

                        if (a == Size.SMALL)
                        {
                            float m = 1.2f;
                            float Price = output.GetSellPrice() + DisassemblePrice - (m * output.GetDisassembleCost());
                            outputgarbageListPrice.Add(Price);
                        }
                        else if (a == Size.MEDIUM)
                        {
                            float m = 1.4f;
                            float Price = output.GetSellPrice() + DisassemblePrice - (m * output.GetDisassembleCost());
                            outputgarbageListPrice.Add(Price);
                        }
                        else if (a == Size.LARGE)
                        {
                            float m = 1.6f;
                            float Price = output.GetSellPrice() + DisassemblePrice - (m * output.GetDisassembleCost());
                            outputgarbageListPrice.Add(Price);
                        }



                    }
                    else if (garbageList[i] is GlassBottleGarbage)
                    {

                        GlassBottleGarbage output = (GlassBottleGarbage)garbageList[i];
                        float Price = output.GetSellPrice();

                        outputgarbageListPrice.Add(Price);
                    }
                    else
                    {
                        MaterialGarbage output = (MaterialGarbage)garbageList[i];
                        float Price = output.GetSellPrice();

                        outputgarbageListPrice.Add(Price);
                    }
                }



            }
            float MaxSellPrice = 0;
            foreach (float Max in outputgarbageListPrice)
            {

                if (Max > MaxSellPrice)
                {
                    MaxSellPrice = Max;
                }


            }

            return MaxSellPrice;
        }
    }

    class Program
    {
        static List<Garbage> GetInputGarbageList(){
            List<Garbage> inputGarbageList = new List<Garbage>();
            inputGarbageList.Add( new Garbage( "paper", Size.SMALL ) );
            inputGarbageList.Add( new ElectronicsGarbage( "television", Size.LARGE, 350.0f ) );
            inputGarbageList.Add( new ElectronicsGarbage( "smartphone", Size.SMALL, 220.0f ) );
            inputGarbageList.Add( new GlassBottleGarbage( "wine_bottle", 15.0f ) );
            inputGarbageList.Add( new MaterialGarbage( "plank", Size.MEDIUM, 0.5f ) );
            inputGarbageList.Add( new ElectronicsGarbage( "motor", Size.SMALL, 120.5f ) );
            return inputGarbageList;
        }

        static void TestCase1(){
            List<Garbage> inputGarbageList = GetInputGarbageList();

            List<Garbage> outputGarbageList = Quiz.SortGarbageListBySize( inputGarbageList, false );

            bool isSort = true;
            Size garbageSize = outputGarbageList[0].GetSize();
            foreach( Garbage garbage in outputGarbageList ){
                if((int)(garbage.GetSize()) < (int)(garbageSize)){
                    isSort = false;
                    break;
                }
                garbageSize = garbage.GetSize();
            }

            Debug.Assert(isSort);
        }

        static void TestCase2(){
            List<Garbage> inputGarbageList = GetInputGarbageList();

            List<Garbage> outputGarbageList = Quiz.SortGarbageListBySize( inputGarbageList, true );

            bool isSort = true;
            Size garbageSize = outputGarbageList[0].GetSize();
            foreach( Garbage garbage in outputGarbageList ){
                if((int)(garbage.GetSize()) > (int)(garbageSize)){
                    isSort = false;
                    break;
                }
                garbageSize = garbage.GetSize();
            }

            Debug.Assert(isSort);
        }

        static void TestCase3(){
            List<Garbage> inputGarbageList = GetInputGarbageList();

            List<Garbage> outputGarbageList = Quiz.GetSellableGarbageList( inputGarbageList );

            List<string> outputGarbageNameList = new List<string>();
            foreach( Garbage garbage in outputGarbageList ){
                outputGarbageNameList.Add( garbage.Label );
            }

            Debug.Assert(outputGarbageNameList.Contains( "television" ));
            Debug.Assert(outputGarbageNameList.Contains( "smartphone" ));
            Debug.Assert(outputGarbageNameList.Contains( "wine_bottle" ));
            Debug.Assert(outputGarbageNameList.Contains( "plank" ));
            Debug.Assert(outputGarbageNameList.Contains( "motor" ));
        }

        static void TestCase4(){
            List<Garbage> inputGarbageList = GetInputGarbageList();

            float outputSellPrice = Quiz.ComputeMaximumSellPrice( inputGarbageList );

            Debug.Assert(outputSellPrice == 1270.725f);
        }

        static void Main(string[] args)
        {
            TestCase1();
            TestCase2();
            TestCase3();
            TestCase4();
        }
    }
}
