using System.Collections.Generic;
using System.Linq;
using CORE.Scripts;
using CORE.Scripts.Game_Rules;
using Letters;
using Scenes._00_Bootstrapper;
using Scenes._50_Minigames._54_SymbolEater.Scripts;
using Scenes._50_Minigames._54_SymbolEater.Scripts.Gamemodes;
using UnityEngine;
using UnityEngine.Rendering;

public class SymbolEaterLevel3 : ISEGameMode
{

        IGameRules gameRules;

        /// <summary>
        /// Should be retrieved from Boardcontroller with method SetLetterCubesAndBoard
        /// </summary>
        List<LetterCube> letterCubes;

        List<LetterCube> activeLetterCubes = new List<LetterCube>();

        int numberOfCorrectLettersOnBoard;

        BoardController boardController;

        int correctLetters = 0;

        int maxWrongLetters = 10;

        int minWrongLetters = 1;

        int maxCorrectLetters = 5;

        int minCorrectLetters = 1;


        /// <summary>
        /// Activates the given cube
        /// </summary>
        /// <param name="letterCube">The lettercube to be activated</param>
        /// <param name="correct">Whether the symbol should be correct</param>
        public void ActivateCube(LetterCube letterCube, bool correct)
        {
            if (correct)
            {
                letterCube.Activate(gameRules.GetCorrectAnswer(), true);
                numberOfCorrectLettersOnBoard++;
            }
            else
            {
                letterCube.Activate(gameRules.GetWrongAnswer());
            }
        }

        /// <summary>
        /// Gets the shown letters for the current game and the correct one
        /// </summary>
        public void GetSymbols()
        {
            gameRules.SetCorrectAnswer();
            if(gameRules.GetType() == typeof(DynamicGameRules))
            {
                DynamicGameRules dynamicGameRules = (DynamicGameRules)gameRules;
                dynamicGameRules.UseFirstVowel();
            }
            //deactives all current active lettercubes
            foreach (LetterCube lC in activeLetterCubes)
            {
                lC.Deactivate();
            }
            int count = Random.Range(minWrongLetters, maxWrongLetters + 1);
            activeLetterCubes.Clear();
            //finds new letterboxes to be activated and assigns them a random wrong letter.
            GameModeHelper.ActivateLetterCubes(count, letterCubes, activeLetterCubes, ActivateCube, false, gameRules, boardController.GetPlayer().transform.position);
            //creates a random number of correct letters on the board
            count = 1;
            GameModeHelper.ActivateLetterCubes(count, letterCubes, activeLetterCubes, ActivateCube, true, gameRules, boardController.GetPlayer().transform.position);
            Texture2D answerImage = ImageManager.GetImageFromWord(gameRules.GetSecondaryAnswer());
            boardController.SetImage(Sprite.Create(answerImage, new Rect(0.0f, 0.0f, answerImage.width, answerImage.height), new Vector2(0.5f, 0.5f), 100.0f));
            boardController.SetAnswerText("Find vokalen i billedet");
        }

    /// <summary>
    /// Checks if the letter is the same as the correct one
    /// </summary>
    /// <param name="letter">The letter which should be checked</param>
    /// <returns>Whether the letter is the correct one</returns>
        public bool IsCorrectSymbol(string letter)
        {
            return gameRules.IsCorrectSymbol(letter);
        }

        /// <summary>
        /// Checks whether there are still correct letters on the board in order to determine if the current game is over
        /// </summary>
        /// <returns>if there are correct letters on the board</returns>
        public bool IsGameComplete()
        {
            if (numberOfCorrectLettersOnBoard == 0)
            {
                return true;
            }
            else
            {
                return false;
            }
        }

        /// <summary>
        /// Replaces an active lettercube with another one
        /// </summary>
        /// <param name="letter">The letter which should be replaced</param>
        public void ReplaceSymbol(LetterCube letter)
        {
            //Checks if the symbol on the lettercube is the correct one
            if (IsCorrectSymbol(letter.GetLetter()))
            {
                numberOfCorrectLettersOnBoard--;
            }
            //Checks if the current game is over or if it should continue the current game
            if (!GameModeHelper.ReplaceOrVictory(letter, letterCubes, activeLetterCubes, false, ActivateCube, IsGameComplete))
            {
                //Checks if the player has won. If not a new game is started
                correctLetters++;
                boardController.monsterHivemind.IncreaseMonsterSpeed();
                if (correctLetters < 1)
                {
                    boardController.monsterHivemind.ResetSpeed();
                    GetSymbols();
                }
                else
                {
                    foreach (LetterCube letterCube in activeLetterCubes)
                    {
                        letterCube.Deactivate();
                    }
                    //Calculates the multiplier for the xp reward. All values are temporary
                    int multiplier = 1;
                    switch (boardController.difficultyManager.diffculty)
                    {
                        case DiffcultyPreset.CUSTOM:
                        case DiffcultyPreset.EASY:
                            multiplier = 1;
                            break;
                        case DiffcultyPreset.MEDIUM:
                            multiplier = 2;
                            break;
                        case DiffcultyPreset.HARD:
                            multiplier = 4;
                            break;
                    }
                    boardController.Won("Du vandt. Du fandt den rigtige vokal 4 gange", multiplier * 1, multiplier * 1);
                }
            }
        }

        /// <summary>
        /// Sets the game rules used by the game mode
        /// </summary>
        /// <param name="gameRules">The game rules to be used</param>
        public void SetGameRules(IGameRules gameRules)
        {
            this.gameRules = gameRules;
        }

        /// <summary>
        /// Gets the list of lettercubes and the boardController from the boardcontroller
        /// </summary>
        /// <param name="letterCubes">List of lettercubes</param>
        /// <param name="board">the board connected to the lettercubes</param>
        public void SetLetterCubesAndBoard(List<LetterCube> letterCubes, BoardController board)
        {
            this.letterCubes = letterCubes;
            boardController = board;
            /*foreach(LetterCube letter in this.letterCubes)
            {
                letter.randomizeFont = true;
            }*/
        }

        /// <summary>
        /// Sets the minimum and maximum correct letters which appears on the board
        /// </summary>
        /// <param name="min"></param>
        /// <param name="max"></param>
        public void SetMinAndMaxCorrectSymbols(int min, int max)
        {
            minCorrectLetters = min;
            maxCorrectLetters = max;
        }

        /// <summary>
        /// Sets the minimum and maximum wrong letters which appears on the board
        /// </summary>
        /// <param name="min"></param>
        /// <param name="max"></param>
        public void SetMinAndMaxWrongSymbols(int min, int max)
        {
            minWrongLetters = min;
            maxWrongLetters = max;
        }

        /// <summary>
        /// Not used
        /// </summary>
        public void UpdateLanguageUnitWeight()
        {
            
        }
}
