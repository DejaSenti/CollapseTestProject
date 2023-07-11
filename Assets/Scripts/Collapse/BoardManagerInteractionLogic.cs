using System.Collections.Generic;
using Collapse.Blocks;

namespace Collapse {
    /**
     * Partial class for separating the main functions that are needed to be modified in the context of this test
     */
    public partial class BoardManager {
        
        /**
         * Trigger a bomb
         */
        public void TriggerBomb(Bomb bomb) {
            //TODO: Implement
        }

        /**
         * Trigger a match
         */
        public void TriggerMatch(Block block) {
            // Find all blocks in this match
            var results = new List<Block>();
            var tested = new List<(int row, int col)>();
            FindChainRecursive(block.Type, block.GridPosition.x, block.GridPosition.y, tested, results);
            
            // Trigger blocks
            for (var i = 0; i < results.Count; i++) {
                results[i].Triger(i);
            }

            // Regenerate
            ScheduleRegenerateBoard();
        }


        /**
         * Recursively collect all neighbors of same type to build a full list of blocks in this "chain" in the results list
         */
        private void FindChainRecursive(BlockType type, int col, int row, List<(int row, int col)> testedPositions,
            List<Block> results) {

            testedPositions.Add((col, row));
            results.Add(blocks[col, row]);

            (int colOffset, int rowOffset)[] offsets = new (int colOffset, int rowOffset)[]
            {
                (1, 0),
                (0, 1),
                (-1, 0),
                (0, -1)
            };

            foreach (var (colOffset, rowOffset) in offsets)
            {
                var newPos = (col + colOffset, row + rowOffset);

                if (newPos.Item1 < 0 || newPos.Item1 >= BoardSize.x || newPos.Item2 < 0 || newPos.Item2 >= BoardSize.y)
                    continue;

                if (blocks[newPos.Item1, newPos.Item2].Type != type || testedPositions.Contains(newPos))
                    continue;

                FindChainRecursive(blocks[col, row].Type, newPos.Item1, newPos.Item2, testedPositions, results);
            }
        }
    }
}