using microController.graphics;
using microController.helpers;
using SFML.Graphics;
using SFML.System;
using SFML.Window;
using System;

namespace microController.game
{
    public static class PauseMenu
    {
        private static Rectangle Rectangle;

        private static Button ResumeButton;
        private static Button SettingsButton;
        private static Button ExitButton;

        private static int numButtons = 3;
        private static int spacingButtons = 30;

        private static Game Game;

        public static bool IsSettingsOpen = false;

        public static void Init(Game game)
        {
            Rectangle = new Rectangle(0, 0, (Vector2f)game.Window.Size) { FillColor = new Color(35, 35, 35, 200) };

            ResumeButton = new Button(new Vector2f(), new Vector2f(), "Resume", MessegeManager.Courier, 16);
            SettingsButton = new Button(new Vector2f(), new Vector2f(), "Settings", MessegeManager.Courier, 16);
            ExitButton = new Button(new Vector2f(), new Vector2f(), "Exit", MessegeManager.Courier, 16);

            ResumeButton.ButtonClicked += ResumeButton_ButtonClicked;
            SettingsButton.ButtonClicked += SettingsButton_ButtonClicked;
            ExitButton.ButtonClicked += ExitButton_ButtonClicked;

            Settings.Init(game);
        }

        private static void UpdatePauseMenu(Game game)
        {
            Game = game;
            Rectangle.PositionX = 0f; Rectangle.PositionY = 0f;
            Rectangle.Size = (Vector2f)game.Window.Size;

            float buttonPositionOffsetX = game.Window.Size.X / 4;
            float buttonPositionOffsetY = game.Window.Size.Y / 5;

            float buttonSizeX = game.Window.Size.X / 2;
            float buttonSizeY = (game.Window.Size.Y - 2 * buttonPositionOffsetY - ((numButtons - 1) * spacingButtons)) / numButtons;

            ResumeButton.Update(Mouse.GetPosition() - game.Window.Position - MouseManager.MouseOffSet, Mouse.IsButtonPressed(Mouse.Button.Left));
            ResumeButton.PositionX = buttonPositionOffsetX;
            ResumeButton.PositionY = buttonPositionOffsetY;
            ResumeButton.SizeX = buttonSizeX;
            ResumeButton.SizeY = buttonSizeY;
            ResumeButton.Text.CharacterSize = (uint)buttonSizeY / 3;

            SettingsButton.Update(Mouse.GetPosition() - game.Window.Position - MouseManager.MouseOffSet, Mouse.IsButtonPressed(Mouse.Button.Left));
            SettingsButton.PositionX = buttonPositionOffsetX;
            SettingsButton.PositionY = buttonPositionOffsetY + buttonSizeY + spacingButtons;
            SettingsButton.SizeX = buttonSizeX;
            SettingsButton.SizeY = buttonSizeY;
            SettingsButton.Text.CharacterSize = (uint)buttonSizeY / 3;

            ExitButton.Update(Mouse.GetPosition() - game.Window.Position - MouseManager.MouseOffSet, Mouse.IsButtonPressed(Mouse.Button.Left));
            ExitButton.PositionX = buttonPositionOffsetX;
            ExitButton.PositionY = buttonPositionOffsetY + (buttonSizeY * (numButtons - 1)) + (spacingButtons * (numButtons - 1));
            ExitButton.SizeX = buttonSizeX;
            ExitButton.SizeY = buttonSizeY;
            ExitButton.Text.CharacterSize = (uint)buttonSizeY / 3;
        }

        public static void Update(Game game)
        {
            if (IsSettingsOpen)
            {
                Settings.Update(game);
            }
            else
            {
                UpdatePauseMenu(game);
            }
        }

        private static void DrawPauseMenu(RenderWindow window)
        {
            Rectangle.Draw(window);
            ResumeButton.Draw(window);
            SettingsButton.Draw(window);
            ExitButton.Draw(window);
        }

        public static void Draw(RenderWindow window)
        {
            if (IsSettingsOpen)
            {
                Settings.Draw(window);
            }
            else
            {
                DrawPauseMenu(window);
            }
        }

        private static void ResumeButton_ButtonClicked(object source, EventArgs args)
        {
            Game.IsGamePaused = false;
        }

        private static void SettingsButton_ButtonClicked(object source, EventArgs args)
        {
            IsSettingsOpen = true;
        }

        private static void ExitButton_ButtonClicked(object source, EventArgs args)
        {
            Game.Window.Close();
        }

        public static void OpenClosePauseMenu(Game game)
        {
            if (KeyboardManager.OnKeyPressed(Keyboard.Key.Escape))
            {
                if (!IsSettingsOpen)
                {
                    if (game.IsGamePaused)
                    {
                        game.IsGamePaused = false;
                    }
                    else
                    {
                        game.IsGamePaused = true;
                    }
                }
                else
                {
                    IsSettingsOpen = false;
                }
            }
        }
    }
}
