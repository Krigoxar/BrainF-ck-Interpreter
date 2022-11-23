using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.ComponentModel;
using System.Runtime.CompilerServices;
using System.Collections.ObjectModel;
using System.Threading;

namespace BrainFuck.ViewModel
{
    internal class MainWindowViewModel : INotifyPropertyChanged
    {
        private string sourceCodeText { get; set; }
        private string inputText { get; set; }
        private string outputText { get; set; }

        private Model.RelayCommand brainFuckCommand;
        public Model.RelayCommand BrainFuckCommand
        {
            get
            {
                return brainFuckCommand ??
                  (brainFuckCommand = new Model.RelayCommand(obj =>
                  {
                      Task.Run(() => ExecBrainFuckProgramm(SourceCodeText, InputText));
                  }));
            }
        }
        public string SourceCodeText
        {
            get { return sourceCodeText; }
            set
            {
                sourceCodeText = value;
                OnPropertyChanged("SourceCodeText");
            }
        }
        public string InputText
        {
            get { return inputText; }
            set
            {
                inputText = value;
                OnPropertyChanged("InputText");
            }
        }
        public string OutputText
        {
            get { return outputText; }
            set
            {
                outputText = value;
                OnPropertyChanged("OutputText");
            }
        }
        public MainWindowViewModel()
        {

        }
        public event PropertyChangedEventHandler PropertyChanged;
        public void OnPropertyChanged([CallerMemberName] string prop = "")
        {
            if (PropertyChanged != null)
                PropertyChanged(this, new PropertyChangedEventArgs(prop));
        }

        private void ExecBrainFuckProgramm(string Code, string Input = "")
        {
            int PlaceInCode = 0;
            int[] MemArray = new int[30000];
            int ArrPointer = 0;
            int BracketCounter = 0;

            int PlaceInInput = 0;

            string Output = "";
            OutputText = "";

            while (Code.Length != PlaceInCode)
            {
                switch (Code[PlaceInCode])
                {
                    case '>': ArrPointer++; break;
                    case '<': ArrPointer--; break;
                    case '+': MemArray[ArrPointer]++; break;
                    case '-': MemArray[ArrPointer]--; break;
                    case '[':
                        if (MemArray[ArrPointer] != 0)
                        {
                            PlaceInCode++;
                            continue; 
                        }
                        ++BracketCounter;
                        while (BracketCounter != 0)
                            switch (Code[++PlaceInCode])
                            {
                                case '[': ++BracketCounter; break;
                                case ']': --BracketCounter; break;
                            }
                        break;
                    case ']':
                        if (MemArray[ArrPointer] == 0)
                        {
                            PlaceInCode++;
                            continue;
                        }
                        ++BracketCounter;
                        while (BracketCounter != 0)
                            switch (Code[--PlaceInCode])
                            {
                                case '[': --BracketCounter; break;
                                case ']': ++BracketCounter; break;
                            }
                        --PlaceInCode;
                        break;
                    case ',':
                        if (PlaceInCode == Input.Length)
                        {

                        }
                        else
                        {
                            MemArray[ArrPointer] = Input[PlaceInInput];
                        }
                        PlaceInInput++;
                        break;
                    case '.': 
                        Output += (char)MemArray[ArrPointer];
                        OutputText = Output;
                        break;
                    default:
                        break;
                }
                PlaceInCode++;
                //Thread.Sleep(10);
            }
        }
    }
}
