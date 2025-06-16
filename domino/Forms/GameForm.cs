using System;
using System.Collections;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Diagnostics.Eventing.Reader;
using System.Drawing;
using System.Linq;
using System.Media;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using domino.Properties;
using static System.Windows.Forms.VisualStyles.VisualStyleElement.Rebar;
using System.Xml.Serialization;
using System.IO;
using domino.Players;

namespace domino
{
    public partial class GameForm : Form
    {
        public int _playerCount;
        public int _currentTurn;
        private int _selectedTileIndex = -1;
        private bool _isGameOver = false;
        public List<Player> _players;
        public List<DominoTile> _stock;
        public List<DominoTile> _board;
        private DominoTileControl _selectedTileControl;
        SoundPlayer _soundPlayer = new SoundPlayer(Properties.Resources.sound);
        private FlowLayoutPanel player2Tiles;
        private FlowLayoutPanel player3Tiles;
        private FlowLayoutPanel player4Tiles;
        private FlowLayoutPanel playerTilesPanel;
        private PictureBox boardPictureBox;
        private Label currentTurnLabel;
        private Label stockLabel;
        private Button skipTurnButton;
        private Button drawTileButton;

        public GameForm(int playerCount, string aiDifficulty)
        {
            InitializeComponent();
            InitializeUI();
            InitializeGame(playerCount, aiDifficulty);
            this.KeyPreview = true;
            this.KeyDown += GameForm_KeyDown;
            this.FormClosing += GameForm_FormClosing;
        }

        private void InitializeUI()
        {
            this.Size = new Size(1280, 800);

            // Игровое поле
            boardPictureBox = new PictureBox
            {
                Size = new Size(1000, 240),
                Location = new Point(132, 200),
                BorderStyle = BorderStyle.FixedSingle,
                BackColor = Color.Transparent
            };
            this.Controls.Add(boardPictureBox);

            // Панель для игрока 1 (нижний)
            playerTilesPanel = new FlowLayoutPanel
            {
                Dock = DockStyle.Bottom,
                Height = 130,
                AutoScroll = true
            };
            this.Controls.Add(playerTilesPanel);

            // Панель для игрока 2 (верхний)
            player2Tiles = new FlowLayoutPanel
            {
                Dock = DockStyle.Top,
                Height = 130,
                AutoScroll = true
            };
            this.Controls.Add(player2Tiles);

            // Панель для игрока 3 (левый)
            player3Tiles = new FlowLayoutPanel
            {
                Dock = DockStyle.Left,
                Width = 130,
                AutoScroll = true,
                FlowDirection = FlowDirection.TopDown // Вертикальное отображение
            };
            this.Controls.Add(player3Tiles);

            // Панель для игрока 4 (правый)
            player4Tiles = new FlowLayoutPanel
            {
                Dock = DockStyle.Right,
                Width = 130,
                AutoScroll = true,
                FlowDirection = FlowDirection.TopDown // Вертикальное отображение
            };
            this.Controls.Add(player4Tiles);

            // Метка текущего хода
            currentTurnLabel = new Label
            {
                Text = "Ход: Игрок 1",
                Dock = DockStyle.Top,
                Font = new Font("Arial", 12, FontStyle.Bold)
            };
            this.Controls.Add(currentTurnLabel);

            // Метка остатка в базаре
            stockLabel = new Label
            {
                Text = "Осталось в базаре: 28",
                Dock = DockStyle.Bottom,
                Font = new Font("Arial", 10)
            };
            this.Controls.Add(stockLabel);

            skipTurnButton = new Button
            {
                Text = "Пропустить ход (Q)",
                Dock = DockStyle.Bottom,
                Height = 50,
                Enabled = false
            };
            skipTurnButton.Click += SkipTurnButton_Click;
            this.Controls.Add(skipTurnButton);

            drawTileButton = new Button
            {
                Text = "Пойти на базар (E)",
                Dock = DockStyle.Bottom,
                Height = 50,
                Enabled = false
            };
            drawTileButton.Click += DrawTileButton_Click;
            this.Controls.Add(drawTileButton);
        }

        private void InitializeGame(int playerCount, string aiDifficulty,bool isLoadedFromSave = false)
        {
            if (isLoadedFromSave) {  return ; }
            _playerCount = playerCount; // Инициализируем _playerCount
            _stock = GenerateStock();
            _players = GeneratePlayers(playerCount, aiDifficulty);
            DealTiles();
            _board = new List<DominoTile>();
            _currentTurn = DetermineStartingPlayer(); // Определяем первого игрока
            UpdateUI();
        }

        public GameForm(string filePath)
        {
            InitializeComponent();
            InitializeUI();
            LoadGameData(filePath);
            this.KeyPreview = true;
            this.KeyDown += GameForm_KeyDown;
            this.FormClosing += GameForm_FormClosing;
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

        private void DealTiles()
        {
            Random rnd = new Random();
            if (_playerCount == 2)
            {
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
            else
            {
                foreach (var player in _players)
                {
                    for (int i = 0; i < 5; i++)
                    {
                        int index = rnd.Next(_stock.Count);
                        player.Hand.Add(_stock[index]);
                        _stock.RemoveAt(index);
                    }
                }
            }
        }

        private void UpdateUI()
        {
            Console.WriteLine("Обновление UI");

            // Обновление игрового поля
            boardPictureBox.Image = DrawBoard();
            // Очистка панелей
            playerTilesPanel.Controls.Clear();
            player2Tiles.Controls.Clear();
            player3Tiles.Controls.Clear();
            player4Tiles.Controls.Clear();
            _selectedTileIndex = -1;
            _selectedTileControl = null;
            for (int i = 0; i < _players.Count; i++)
            {
                var player = _players[i];
                var tiles = player.Hand;
                bool isAI = player is AIPlayer;
                bool isRotated = false;
                if (i == 2 || i == 3) isRotated = true;
                //Console.WriteLine($"Игрок {i + 1}: {player.Name}, Количество фишек: {tiles.Count}");

                if (_players[_currentTurn] is HumanPlayer)
                {
                    drawTileButton.Enabled = true;
                    if (_stock.Count == 0)
                    {
                        drawTileButton.Enabled = false;
                        skipTurnButton.Enabled = true;
                    }
                }
                else
                {
                    drawTileButton.Enabled = false;
                    skipTurnButton.Enabled = false;
                }

                    foreach (var tile in tiles)
                    {
                       // Console.WriteLine($"Добавляем фишку {tile.Left}-{tile.Right} для игрока {i + 1}");
                        var control = CreateTileControl(tile, isAI, isRotated);

                        switch (i)
                        {
                            case 0:
                                playerTilesPanel.Controls.Add(control);
                                break;
                            case 1:
                                player2Tiles.Controls.Add(control);
                                break;
                            case 2:
                                player3Tiles.Controls.Add(control);
                                break;
                            case 3:
                                player4Tiles.Controls.Add(control);
                                break;
                        }
                    }
            }

            // Обновление меток
            currentTurnLabel.Text = $"Ход: {_players[_currentTurn].Name}";
            stockLabel.Text = $"Осталось в базаре: {_stock.Count}";
        }

        private Image DrawBoard()
        {
            if (_board.Count == 0) return null;

            int tileWidth = 60;
            int tileHeight = 30;
            int spacing = 5;
            int maxHorizontal = 16; // Максимальное количество горизонтальных фишек
            int maxVertical = 4;    // Максимальное количество вертикальных фишек

            // Разделение фишек на секции
            int horizontalCount = Math.Min(_board.Count, maxHorizontal);
            int verticalCount = Math.Min(Math.Max(_board.Count - horizontalCount, 0), maxVertical);
            int remainingCount = Math.Max(0, _board.Count - horizontalCount - verticalCount);

            // Размеры секций
            int horizontalWidth = horizontalCount * tileWidth + (horizontalCount - 1) * spacing;
            int verticalWidth = tileHeight; // Ширина вертикальной секции (после поворота)
            int verticalHeight = verticalCount * tileWidth + (verticalCount - 1) * spacing;
            int remainingWidth = remainingCount * tileWidth + (remainingCount - 1) * spacing;

            // Общие размеры Bitmap
            int bmpWidth = horizontalWidth + verticalWidth + remainingWidth + 2 * spacing;
            int bmpHeight = Math.Max(tileHeight, verticalHeight);

            Bitmap bmp = new Bitmap(bmpWidth, bmpHeight);
            Graphics g = Graphics.FromImage(bmp);
            g.Clear(Color.Transparent);

            int x = 0, y = 0;

            // Отрисовка первой секции (горизонтальные слева направо)
            for (int i = 0; i < horizontalCount; i++)
            {
                DrawTile(g, _board[i], x, y, tileWidth, tileHeight);
                x += tileWidth;
            }

            // Отрисовка второй секции (вертикальные сверху вниз)
          //  x = horizontalWidth;
            for (int i = horizontalCount; i < horizontalCount + verticalCount; i++)
            {
                DrawTile(g, _board[i], x, y, tileWidth, tileHeight, RotateFlipType.Rotate90FlipNone);
                y += tileWidth; // Высота фишки после поворота = tileWidth
            }

            // Отрисовка третьей секции (горизонтальные справа налево)
            // x = bmpWidth - remainingWidth - spacing;
            // y = tileWidth;
            x = x - tileWidth;
            y = y - (tileWidth / 2);
            for (int i = horizontalCount + verticalCount; i < _board.Count; i++)
            {
                DrawTile(g, _board[i], x, y, tileWidth, tileHeight, RotateFlipType.Rotate180FlipNone);
                x -= tileWidth; // Движение влево
            }

            return bmp;
        }

        private void DrawTile(Graphics g, DominoTile tile, int x, int y,
                              int width, int height, RotateFlipType rotate = RotateFlipType.RotateNoneFlipNone)
        {
            var control = new DominoTileControlBoard(tile);
            Image img = control.PictureBox1.Image;

            // Масштабируем изображение
            if (img.Width != width || img.Height != height)
                img = new Bitmap(img, new Size(width, height));

            // Поворачиваем, если нужно
            if (rotate != RotateFlipType.RotateNoneFlipNone)
            {
                img.RotateFlip(rotate);
            }

            g.DrawImage(img, x, y);
        }

        private void OnTileClicked(DominoTile tile)
        {
            if (_players[_currentTurn] is AIPlayer) return;
            if (CanPlaceTile(tile))
            {
                var control = new DominoTileControl(tile);
                var img = control.PictureBox1.Image;
                _players[_currentTurn].Hand.Remove(tile);

                // Если доска пуста — добавляем фишку как первую
                if (_board.Count == 0)
                {
                    _soundPlayer.Play();
                    _board.Add(tile);
                }
                else
                {
                    // Добавляем к правой или левой части
                    int leftEnd = _board.First().Left;
                    int rightEnd = _board.Last().Right;
                    int buffer = 0;
                    if ((tile.Right == rightEnd && tile.Right == leftEnd) || (tile.Left == rightEnd && tile.Left == leftEnd) || 
                        (tile.Right == leftEnd && tile.Left == rightEnd) || (tile.Left == leftEnd && tile.Right == rightEnd))
                    {
                        ChooseDialog dlg = new ChooseDialog();
                        if(dlg.ShowDialog() == DialogResult.OK)
                        {
                            if (tile.Right == leftEnd)
                            {
                                _soundPlayer.Play();
                                _board.Insert(0, tile); // Добавляем в начало, если подходит левый конец
                            }
                            else if (tile.Left == leftEnd)
                            {
                                buffer = tile.Right;
                                tile.Right = tile.Left;
                                tile.Left = buffer;
                                control.RotateImage(img, RotateFlipType.Rotate180FlipNone);
                                _soundPlayer.Play();
                                _board.Insert(0, tile);
                            }
                        }
                        else
                        {
                            if (tile.Left == rightEnd)
                            {
                                _soundPlayer.Play();
                                _board.Add(tile);
                            }
                            else if (tile.Right == rightEnd)
                            {
                                buffer = tile.Left;
                                tile.Left = tile.Right;
                                tile.Right = buffer;
                                control.RotateImage(img, RotateFlipType.Rotate180FlipNone);
                                _soundPlayer.Play();
                                _board.Add(tile);
                            }
                        }
                    }
                    else if (tile.Left == rightEnd)
                    {
                        _soundPlayer.Play();
                        _board.Add(tile);
                    }
                    else if (tile.Right == rightEnd)
                    {
                        buffer = tile.Left;
                        tile.Left = tile.Right;
                        tile.Right = buffer;
                        control.RotateImage(img, RotateFlipType.Rotate180FlipNone);
                        _soundPlayer.Play();
                        _board.Add(tile);
                    }
                    else if (tile.Right == leftEnd)
                    {
                        _soundPlayer.Play();
                        _board.Insert(0, tile); // Добавляем в начало, если подходит левый конец
                    }
                    else if (tile.Left ==  leftEnd)
                    {
                        buffer = tile.Right;
                        tile.Right = tile.Left;
                        tile.Left = buffer;
                        control.RotateImage(img,RotateFlipType.Rotate180FlipNone);
                        _soundPlayer.Play();
                        _board.Insert(0, tile);
                    }
                }
                UpdateUI();
                NextTurn();
            }
        }

        private void NextTurn()
        {
            _currentTurn = (_currentTurn + 1) % _players.Count;

            if (_players[_currentTurn] is AIPlayer aiPlayer && !CheckWin())
            {
                MakeAIPlay();
            }
            UpdateUI();
            CheckWin();
        }

        private async void MakeAIPlay()
        {
            await Task.Delay(2000);

            var aiPlayer = (AIPlayer)_players[_currentTurn];
            var chosenTile = aiPlayer.ChooseTile(_stock, _board);
            if (chosenTile != null)
            {
                aiPlayer.Hand.Remove(chosenTile);
                if (_board.Count == 0)
                {
                    _soundPlayer.Play();
                    _board.Add(chosenTile);
                    UpdateUI();
                    NextTurn();
                }
                else {
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
                    UpdateUI();
                    NextTurn();
                }
            }
            else
            {
                UpdateUI();
                NextTurn();
            }
        }

        private bool CanPlaceTile(DominoTile tile)
        {
            if (_board.Count == 0) return true; // Первая фишка допустима

            // Левый конец доски (левая часть самой левой фишки)
            int leftEnd = _board.First().Left;

            // Правый конец доски (правая часть самой правой фишки)
            int rightEnd = _board.Last().Right;

            return (tile.Left == leftEnd || tile.Right == leftEnd) ||
                   (tile.Left == rightEnd || tile.Right == rightEnd);
        }

        private bool CheckWin()
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
                        this.Hide();
                        return true;
                    }
                    else
                    {
                        Application.Exit();
                        return true;
                    }
                }
            }

            if (_stock.Count == 0 && !CanAnyPlayerMove())
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
                    this.Hide();
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

        private bool CanAnyPlayerMove()
        {
            foreach (var player in _players)
            {
                var validTiles = player.GetValidTiles(_board);
                if (validTiles.Any()) return true;
            }
            return false;
        }

        private int DetermineStartingPlayer()
        {
            // Определяем первого игрока с самой большой фишкой
            int maxSum = -1;
            int startingPlayerIndex = 0;

            for (int i = 0; i < _players.Count; i++)
            {
                var player = _players[i];
                foreach (var tile in player.Hand)
                {
                    int sum = tile.Left + tile.Right;
                    if (sum > maxSum)
                    {
                        maxSum = sum;
                        startingPlayerIndex = i;
                    }
                }
            }
            if (startingPlayerIndex != 0)
            {
                MakeAIPlay();
            }
            return startingPlayerIndex;
        }

        private DominoTileControl CreateTileControl(DominoTile tile, bool isAI, bool isRotated)
        {
            if (isAI)
            {
                var control = new DominoTileControlAI(tile, isRotated);
                return control;
            }
            else
            {
                var control = new DominoTileControl(tile);
                control.TileClicked += (s, e) => OnTileClicked(tile);
                return control;
            }
        }

        private void SaveGameDialog()
        {
            SaveFileDialog saveFileDialog = new SaveFileDialog
            {
                Filter = "Domino save files|*.domino",
                FileName = "save.domino",
                Title = "Сохранить текущую игру"
            };

            if (saveFileDialog.ShowDialog() == DialogResult.OK)
            {
                SaveLoadManager.SaveGame(saveFileDialog.FileName, this);
                MessageBox.Show("Игра сохранена!");
            }
        }

        private void LoadGameDialog()
        {
            OpenFileDialog openFileDialog = new OpenFileDialog
            {
                Filter = "Domino save files|*.domino",
                Title = "Открыть сохраненную игру"
            };

            if (openFileDialog.ShowDialog() == DialogResult.OK)
            {
                LoadGameData(openFileDialog.FileName);
            }
        }

        public void LoadGameData(string filePath)
        {
            var saveData = SaveLoadManager.LoadGame(filePath);
            if (saveData == null) return;

            _playerCount = saveData.PlayerCount;
            _currentTurn = saveData.CurrentTurn;

            _players = new List<Player>();
            foreach (var pData in saveData.Players)
            {
                if (pData.PlayerType == "AIPlayerEasy" || pData.PlayerType == "AIPlayerMedium" || pData.PlayerType == "AIPlayerHard")
                {
                    if (pData.AIDifficulty == "Легкий")
                    {
                        _players.Add(new AIPlayerEasy(pData.Name, pData.AIDifficulty)
                        {
                            Hand = pData.Hand ?? new List<DominoTile>()
                        });
                    }
                    else if (pData.AIDifficulty == "Средний")
                    {
                        _players.Add(new AIPlayerMedium(pData.Name, pData.AIDifficulty)
                        {
                            Hand = pData.Hand ?? new List<DominoTile>()
                        });
                    }
                    if (pData.AIDifficulty == "Сложный")
                    {
                        _players.Add(new AIPlayerHard(pData.Name, pData.AIDifficulty)
                        {
                            Hand = pData.Hand ?? new List<DominoTile>()
                        });
                    }
                }
                else if (pData.PlayerType == "HumanPlayer")
                {
                    _players.Add(new HumanPlayer(pData.Name) 
                    { 
                        Hand = pData.Hand ?? new List<DominoTile>()
                    });
                }
            }
            _stock = saveData.Stock ?? new List<DominoTile>();
            _board = saveData.Board ?? new List<DominoTile>();

            if (_currentTurn >= _players.Count) _currentTurn = 0;
            UpdateUI();
        }
        private void SkipTurnButton_Click(object sender, EventArgs e)
        {
            NextTurn();
        }
        private void DrawTileButton_Click(object sender, EventArgs e)
        {
            Random rnd = new Random();
            int index = rnd.Next(_stock.Count);
            DominoTile takenTile = _stock[index];
            _stock.RemoveAt(index);
            _players[0].Hand.Add(takenTile);
            UpdateUI();
        }
        private void rulesButton_Click(object sender, EventArgs e)
        {
            MessageBox.Show(
                "Ваша задача в игре: выложить все фишки быстрее оппонентов.\nЧтобы выложить фишку на стол, кликните на нее мышкой, или нажмите F, выбрав нужную фишку с помощью кнопок A и D.\nЕсли у вас нет нужной фишки, то нужно сходить на базар (кнопка E).\nЕсли базар пуст и нет подходящей фишки, нужно пропустить ход (кнопка Q).\nУдачи!");
        }
        private void saveButton_Click(object sender, EventArgs e)
        {
            SaveGameDialog();
        }

        private void loadButton_Click(object sender, EventArgs e)
        {
            LoadGameDialog();
        }

        private void GameForm_KeyDown(object sender, KeyEventArgs e)
        {
            if (_players[_currentTurn] is AIPlayer) return;

            var tiles = playerTilesPanel.Controls.OfType<DominoTileControl>().ToList();
            if (tiles.Count == 0) return;

            if (e.KeyCode == Keys.A || e.KeyCode == Keys.D)
            {
                if (_selectedTileControl != null)
                {
                    _selectedTileControl.BorderStyle = BorderStyle.None;
                }

                if (e.KeyCode == Keys.A)
                {
                    _selectedTileIndex = (_selectedTileIndex - 1 + tiles.Count) % tiles.Count;
                }
                else
                {
                    _selectedTileIndex = (_selectedTileIndex + 1) % tiles.Count;
                }

                _selectedTileControl = tiles[_selectedTileIndex];
                _selectedTileControl.BorderStyle = BorderStyle.FixedSingle;
                _selectedTileControl.Focus();
            }
            else if (e.KeyCode == Keys.F && _selectedTileControl != null)
            {
                var tile = _selectedTileControl.Tile;
                OnTileClicked(tile);
            }
            else if ((e.KeyCode == Keys.Q) && skipTurnButton.Enabled)
            {
                SkipTurnButton_Click(sender, e);
            }

            else if ((e.KeyCode == Keys.E) && drawTileButton.Enabled)
            {
                DrawTileButton_Click(sender, e);
            }

            else if(e.KeyCode == Keys.F1)
            {
                rulesButton_Click(sender, e);
            }
            else if(e.KeyCode == Keys.R)
            {
                restartButton_Click(sender, e);
            }

            else if(e.KeyCode == Keys.L)
            {
                loadButton_Click(sender, e);
            }

            else if(e.KeyCode == Keys.S)
            {
                saveButton_Click(sender, e);
            }
        }

        private void restartButton_Click(object sender, EventArgs e)
        {
            DialogResult result = MessageBox.Show(
                    "Вы действительно хотите начать новую игру?",
                    "Начать новую игру?",
                    MessageBoxButtons.OKCancel,
                    MessageBoxIcon.Question,
                    MessageBoxDefaultButton.Button1
                    );
            if (result == DialogResult.OK)
            {
                var mainMenu = new StartForm();
                mainMenu.Show();
                this.Hide();
            }
        }
        private void GameForm_FormClosing(object sender, FormClosingEventArgs e)
        {
            Application.Exit();
            Environment.Exit(0);
        }
    }
}