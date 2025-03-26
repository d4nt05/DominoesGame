using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml.Serialization;
using System.IO;

namespace domino
{
    public static class SaveLoadManager
    {
        [XmlRoot("GameSave")]
        public class GameSaveData
        {

            [XmlElement("PlayerCountr")]
            public int PlayerCount { get; set; }

            [XmlElement("CurrentTurn")]
            public int CurrentTurn { get; set; }

            [XmlArray("Players")]
            [XmlArrayItem("Player")]
            public List<PlayerData> Players { get; set; }

            [XmlArray("Stock")]
            [XmlArrayItem("Tile")]
            public List<DominoTile> Stock { get; set; }

            [XmlArray("Board")]
            [XmlArrayItem("Tile")]
            public List<DominoTile> Board { get; set; }
        }

        public class PlayerData
        {
            [XmlElement("Name")]
            public string Name { get; set; }

            [XmlElement]
            public string PlayerType { get; set; }

            [XmlElement("IsAI")]
            public bool IsAI { get; set; }

            [XmlElement("AIDifficulty")]
            public string AIDifficulty { get; set; }

            [XmlArray("Hand")]
            [XmlArrayItem("Tile")]
            public List<DominoTile> Hand { get; set; }
        }

        public static void SaveGame(string filePath, GameForm gameForm)
        {
            var saveData = new GameSaveData
            {
                PlayerCount = gameForm._playerCount,
                CurrentTurn = gameForm._currentTurn,
                Players = gameForm._players.Select(p => new PlayerData
                {
                    Name = p.Name,
                    PlayerType = p.GetType().Name,
                    IsAI = p is AIPlayer,
                    AIDifficulty = (p is AIPlayer ai) ? ai._difficulty : null,
                    Hand = p.Hand.ToList()
                }).ToList(),
                Stock = gameForm._stock.ToList(),
                Board = gameForm._board.ToList()
            };

            var serializer = new XmlSerializer(typeof(GameSaveData));
            using (var writer = new StreamWriter(filePath))
            {
                serializer.Serialize(writer, saveData);
            }
        }

        public static GameSaveData LoadGame(string filePath)
        {
            if (!File.Exists(filePath)) return null;

            var serializer = new XmlSerializer(typeof(GameSaveData));
            using (var reader = new StreamReader(filePath))
            {
                return (GameSaveData)serializer.Deserialize(reader);
            }
        }
    }

}
