using System;
using System.Collections;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Media;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using domino.Players;

namespace domino


{
    internal class GameMaster
    {
        SoundPlayer _soundPlayer = new SoundPlayer(Properties.Resources.sound);
        int CurrentTurn;
        public GameMaster() { }

        public List<DominoTile> GenerateStock()
        {
            var stock = new List<DominoTile>();
            for (int i = 0; i < 7; i++)
            {
                for (int j = i; j < 7; j++)
                {
                    stock.Add(new DominoTile(i, j));
                }
            }
            return stock;
        }

        public void DealTiles(int playerCount, List<DominoTile> stock, List<Player> _players)
        {
            Random rnd = new Random();
            
            if (playerCount == 2)
            {
                foreach (var player in _players)
                {
                    for (int i = 0; i < 7; i++)
                    {
                        int index = rnd.Next(stock.Count);
                        player.Hand.Add(stock[index]);
                        stock.RemoveAt(index);
                    }
                }
            }
            else
            {
                foreach (var player in _players)
                {
                    for (int i = 0; i < 5; i++)
                    {
                        int index = rnd.Next(stock.Count);
                        player.Hand.Add(stock[index]);
                        stock.RemoveAt(index);
                    }
                }
            }
        }

        public List<Player> GeneratePlayers(int playerCount, string difficulty)
        {
            var players = new List<Player>();
            for (int i = 0; i < playerCount; i++)
            {
                if (i == 0)
                    players.Add(new HumanPlayer("Игрок"));
                else
                {
                    if (difficulty == "Легкий")
                    {
                        players.Add(new AIPlayerEasy($"Бот {i + 1}", difficulty));
                    }
                    else if (difficulty == "Средний")
                    {
                        players.Add(new AIPlayerMedium($"Бот {i + 1}", difficulty));
                    }
                    else if (difficulty == "Сложный")
                    {
                        players.Add(new AIPlayerHard($"Бот {i + 1}", difficulty));
                    }
                }
            }
            return players;
        }

        public int DetermineStartingPlayer(GameForm game, List<Player> _players, int _currentTurn, List<DominoTile> _stock, List<DominoTile> _board, Action UpdateUI, bool _isGameOver)
        {
            // Определяем первого игрока с самой большой фишкой
            int maxSum = -1;
            CurrentTurn = _currentTurn;

            for (int i = 0; i < _players.Count; i++)
            {
                var player = _players[i];
                foreach (var tile in player.Hand)
                {
                    int sum = tile.Left + tile.Right;
                    if (sum > maxSum)
                    {
                        maxSum = sum;
                        CurrentTurn = i;
                    }
                }
            }
            if (CurrentTurn != 0)
            {
                MakeAIPlay(game,_players,CurrentTurn,_stock,_board,UpdateUI,_isGameOver);
            }
            return CurrentTurn;
        }

        public bool CanPlaceTile(DominoTile tile, List<DominoTile> _board)
        {
            if (_board.Count == 0) return true; // Первая фишка допустима

            // Левый конец доски (левая часть самой левой фишки)
            int leftEnd = _board.First().Left;

            // Правый конец доски (правая часть самой правой фишки)
            int rightEnd = _board.Last().Right;

            return (tile.Left == leftEnd || tile.Right == leftEnd) ||
                   (tile.Left == rightEnd || tile.Right == rightEnd);
        }

        public bool CanAnyPlayerMove(List<Player> _players, List<DominoTile> _board)
        {
            foreach (var player in _players)
            {
                var validTiles = player.GetValidTiles(_board);
                if (validTiles.Any()) return true;
            }
            return false;
        }

        public bool CheckWin(bool _isGameOver, List<Player> _players, List<DominoTile> _stock, List<DominoTile> _board,GameForm game)
        {
            if (_isGameOver) return true;
            foreach (var player in _players)
            {
                if (player.Hand.Count == 0)
                {
                    _isGameOver = true;
                    DialogResult result = MessageBox.Show(
                        $"{player.Name} выиграл!\nХотите сыграть еще раз?",
                        "Конец игры!",
                        MessageBoxButtons.OKCancel,
                        MessageBoxIcon.Question,
                        MessageBoxDefaultButton.Button1
                        );
                    if (result == DialogResult.OK)
                    {
                        var mainMenu = new StartForm();
                        mainMenu.Show();
                        game.Hide();
                        return true;
                    }
                    else
                    {
                        Application.Exit();
                        return true;
                    }
                }
            }

            if (_stock.Count == 0 && !CanAnyPlayerMove(_players, _board))
            {
                _isGameOver = true;
                DialogResult result = MessageBox.Show(
                    "Ничья! Базар закончился и никто не может ходить.\nХотите сыграть еще раз?",
                    "Конец игры!",
                    MessageBoxButtons.OKCancel,
                    MessageBoxIcon.Question,
                    MessageBoxDefaultButton.Button1
                    );
                if (result == DialogResult.OK)
                {
                    var mainMenu = new StartForm();
                    mainMenu.Show();
                    game.Hide();
                    return true;
                }
                else
                {
                    Application.Exit();
                    return true;
                }

            }
            return false;
        }

        public void NextTurn(GameForm game, int _currentTurn, bool _isGameOver, List<DominoTile> _board, List<DominoTile> _stock, List<Player> _players, Action updateUI)
        {
            CurrentTurn = (CurrentTurn + 1) % _players.Count;
            ReturnCurrentTurn();
            if (_players[CurrentTurn] is AIPlayer aiPlayer && !CheckWin(_isGameOver, _players, _stock, _board, game))
            {
                MakeAIPlay(game,_players,CurrentTurn,_stock,_board,updateUI,_isGameOver);
            }
            CheckWin(_isGameOver, _players, _stock, _board, game);
            updateUI();
        }

        private async void MakeAIPlay(GameForm game,List<Player> _players, int _currentTurn, List<DominoTile> _stock, List<DominoTile> _board, Action UpdateUI,bool _isGameOver)
        {
            await Task.Delay(2000);

            var aiPlayer = (AIPlayer)_players[CurrentTurn];
            var chosenTile = aiPlayer.ChooseTile(_stock, _board);
            if (chosenTile != null)
            {
                aiPlayer.Hand.Remove(chosenTile);
                if (_board.Count == 0)
                {
                    _soundPlayer.Play();
                    _board.Add(chosenTile);
                    NextTurn(game, CurrentTurn, _isGameOver, _board, _stock, _players, UpdateUI);
                    UpdateUI();
                }
                else
                {
                    var control = new DominoTileControl(chosenTile);
                    var img = control.PictureBox1.Image;
                    int leftEnd = _board.First().Left;
                    int rightEnd = _board.Last().Right;
                    int buffer = 0;
                    if (chosenTile.Left == rightEnd)
                    {
                        _soundPlayer.Play();
                        _board.Add(chosenTile);
                    }
                    else if (chosenTile.Right == rightEnd)
                    {
                        buffer = chosenTile.Left;
                        chosenTile.Left = chosenTile.Right;
                        chosenTile.Right = buffer;
                        control.RotateImage(img, RotateFlipType.Rotate180FlipNone);
                        _soundPlayer.Play();
                        _board.Add(chosenTile);
                    }
                    else if (chosenTile.Right == leftEnd)
                    {
                        _soundPlayer.Play();
                        _board.Insert(0, chosenTile); // Добавляем в начало, если подходит левый конец
                    }
                    else if (chosenTile.Left == leftEnd)
                    {
                        buffer = chosenTile.Right;
                        chosenTile.Right = chosenTile.Left;
                        chosenTile.Left = buffer;
                        control.RotateImage(img, RotateFlipType.Rotate180FlipNone);
                        _soundPlayer.Play();
                        _board.Insert(0, chosenTile);
                    }
                    NextTurn(game, CurrentTurn, _isGameOver, _board, _stock, _players, UpdateUI);
                    UpdateUI();
                }
            }
            else
            {
                NextTurn(game, CurrentTurn, _isGameOver, _board, _stock, _players, UpdateUI);
                UpdateUI();
            }
        }

        public int ReturnCurrentTurn() { 
            return CurrentTurn; 
        }
    }
}
