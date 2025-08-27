using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading.Tasks;

namespace DataVisualizationPlatform.ViewModels
{
    public class MainMenuViewModel : INotifyPropertyChanged
    {
        private List<string> _items;
        private int _currentIndex;

        private double _leftPosition;
        public double LeftPosition
        {
            get => _leftPosition;
            set { _leftPosition = value; OnPropertyChanged(nameof(LeftPosition)); }
        }

        private double _centerPosition;
        public double CenterPosition
        {
            get => _centerPosition;
            set { _centerPosition = value; OnPropertyChanged(nameof(CenterPosition)); }
        }

        private double _rightPosition;
        public double RightPosition
        {
            get => _rightPosition;
            set { _rightPosition = value; OnPropertyChanged(nameof(RightPosition)); }
        }

        private string _leftText;
        public string LeftText
        {
            get => _leftText;
            set { _leftText = value; OnPropertyChanged(nameof(LeftText)); }
        }

        private string _centerText;
        public string CenterText
        {
            get => _centerText;
            set { _centerText = value; OnPropertyChanged(nameof(CenterText)); }
        }

        private string _rightText;
        public string RightText
        {
            get => _rightText;
            set { _rightText = value; OnPropertyChanged(nameof(RightText)); }
        }

        public MainMenuViewModel()
        {
            _items = new List<string> { "第一条", "第二条", "第三条", "第四条" };
            _currentIndex = 0;
            UpdateTexts();
            // 初始位置
            LeftPosition = -300;
            CenterPosition = 150;
            RightPosition = 600;
        }

        private void UpdateTexts()
        {
            CenterText = _items[_currentIndex];
            LeftText = _items[(_currentIndex - 1 + _items.Count) % _items.Count];
            RightText = _items[(_currentIndex + 1) % _items.Count];
        }

        public void MoveNext()
        {
            _currentIndex = (_currentIndex + 1) % _items.Count;
            UpdateTexts();

            // 更新位置用于动画
            LeftPosition = -300;
            CenterPosition = 150;
            RightPosition = 600;
        }

        public void MovePrevious()
        {
            _currentIndex = (_currentIndex - 1 + _items.Count) % _items.Count;
            UpdateTexts();

            LeftPosition = -300;
            CenterPosition = 150;
            RightPosition = 600;
        }

        public event PropertyChangedEventHandler PropertyChanged;
        protected void OnPropertyChanged(string name) =>
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(name));
    }
}