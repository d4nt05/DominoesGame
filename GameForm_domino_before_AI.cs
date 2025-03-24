using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Media;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace domino
{
    public partial class GameForm: Form
    {
        private int _playerCount;
        private int _currentTurn;
        private List<Player> _players;
        private List<DominoTile> _stock;
        private List<DominoTile> _board;

        private FlowLayoutPanel player2Tiles;
        private FlowLayoutPanel player3Tiles;
        private FlowLayoutPanel player4Tiles;


        public GameForm(int playerCount, string aiDifficulty)
        {
            InitializeComponent();
            InitializeUI();
            InitializeGame(playerCount, aiDifficulty);
        }

        private void InitializeUI()
        {
            player2Tiles = new FlowLayoutPanel()
            {
                Dock = DockStyle.Top,
                Height = 120,
                AutoScroll = true
            };
            this.Controls.Add(player2Tiles);

            player3Tiles = new FlowLayoutPanel()
            {
                Dock = DockStyle.Left,
                Width = 500,
                AutoScroll= true,
                FlowDirection = FlowDirection.TopDown
            };
            this.Controls.Add(player3Tiles);

            player4Tiles = new FlowLayoutPanel()
            {
                Dock = DockStyle.Right,
                Width = 500,
                AutoScroll= true,
                FlowDirection = FlowDirection.TopDown
            };
            this.Controls.Add(player4Tiles);
        }


        private void InitializeGame(int playerCount, string aiDifficulty)
        {
            _stock = GenerateStock();
            _players = GeneratePlayers(playerCount, aiDifficulty);
            DealTiles();
            _board = new List<DominoTile>();
            _currentTurn = 0;
            UpdateUI();
        }

        private List<DominoTile> GenerateStock()
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
        private List<Player> GeneratePlayers(int playerCount, string difficulty)
        {
            var players = new List<Player>();
            for (int i = 0; i < playerCount; i++)
            {
                if (i == 0) players.Add(new HumanPlayer("Игрок 1"));
                else players.Add(new AIPlayer($"AI {i + 1}",difficulty));
            }
            return players;
        }
        private void DealTiles()
        {
            Random rnd = new Random();
            foreach (var player in _players)
            {
                for (int i = 0; i < 7; i++)
                {
                    int index = rnd.Next(_stock.Count);
                    player.Hand.Add(_stock[index]);
                    _stock.RemoveAt(index);
                }
            }
        }
        private void UpdateUI()
        {
            playerTilesPanel.Controls.Clear();
            player2Tiles.Controls.Clear();
            player3Tiles.Controls.Clear();
            player4Tiles.Controls.Clear();

            for (int i = 0; i < _playerCount; i++)
            {
                var player = _players[i];
                var tiles = player.Hand;
                bool isAI = !(player is HumanPlayer);
                bool isLeftOrRight = i == 2 || i == 3;

                foreach (var tile in tiles)
                {
                    var control = CreateTileControl(tile, isAI, isLeftOrRight);
                    switch (i)
                    {
                        case 0:
                            playerTilesPanel.Controls.Add(control);
                            break;
                        case 1:
                            player2Tiles.Controls.Add(control);
                            break;
                        case 2:;
                            player3Tiles.Controls.Add(control);
                            break;
                        case 3:
                            player4Tiles.Controls.Add(control);
                            break;
                    }
                }
            }
            currentTurnLabel.Text = $"Ход игрока: {_players[_currentTurn].Name}";
            stockLabel.Text = $"Осталось в базаре: {_stock.Count}";
        }
        private DominoTileControl CreateTileControl(DominoTile tile, bool isAI, bool isLeftOrRight)
        {
            var control = new DominoTileControl(tile);
            control.IsAI = isAI;
            control.IsRotated = isLeftOrRight;
            if (!isAI)
            {
                control.TileClicked += (s, e) => OnTileClicked(tile);
            }
            return control;
        }
        
        private void OnTileClicked(DominoTile tile)
        {
            if (CanPlaceTile(tile))
            {
                _players[_currentTurn].Hand.Remove(tile);
                _board.Add(tile);
                UpdateUI();
                NextTurn();
            }
        }
        private void NextTurn()
        {
            _currentTurn = (_currentTurn + 1) % _players.Count;

            if (_players[_currentTurn] is AIPlayer aiPlayer)
            {
                MakeAIPlay();
            }
            UpdateUI();
            CheckWin();
        }
        private void MakeAIPlay()
        {
            var aiPlayer = (AIPlayer)_players[_currentTurn];
            var chosenTile = aiPlayer.ChooseTile(_stock,_board);

            if (chosenTile != null)
            {
                aiPlayer.Hand.Remove(chosenTile);
                _board.Add(chosenTile);
                UpdateUI();
                NextTurn();
            }
            else {
                UpdateUI();
                NextTurn();
            }
        }
        private bool CanPlaceTile(DominoTile tile)
        {
            if (_board.Count == 0) return true;

            int lastTileEnd = _board.Last().Right;
            return tile.Left == lastTileEnd || tile.Right == lastTileEnd;
        }
        private bool CheckWin()
        {
            foreach (var player in _players)
            {
                if (player.Hand.Count == 0)
                {
                    MessageBox.Show($"{player.Name} выиграл!");
                    return true;
                }
            }
            if (_stock.Count == 0 && !CanAnyPlayerMove())
            {
                MessageBox.Show("Ничья! Базар закончился и никто не может ходить.");
                Application.Exit();
                return true;
            }
            return false;
        }
        private bool CanAnyPlayerMove()
        {
            foreach (var player in _players)
            {
                var validTiles = player.GetValidTiles(_board);
                if (validTiles.Any()) return true;
            }
            return false;
        }

    }
}
