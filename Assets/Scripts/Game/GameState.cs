using System.Collections.Generic;
using Game.Field;

namespace Game
{
    [System.Serializable]
    public struct GameState
    {
        public GameState(int width, int height, List<Cell> cells, int highScore)
        {
            this.width = width;
            this.height = height;
            this.cells = cells;
            this.highScore = highScore;
        }

        public int width;
        public int height;
    
        public List<Cell> cells;
        public int highScore;
    }
}