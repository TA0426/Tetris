namespace Tetris;

using System;
using System.Drawing;
using System.Windows.Forms;

public partial class Tetris : Form
{
    private readonly Timer timer;
    private const int GridWidth = 10;
    private const int GridHeight = 20;
    private const int CellSize = 30;
    private readonly int[,] grid = new int[GridWidth, GridHeight];

    private Shape currentShape;
    private Shape nextShape;
    private int currentX;
    private int currentY;
    private int score;

    public Tetris()
    {
        InitializeComponent();

        ClientSize = new Size(540, 650);
        DoubleBuffered = true;
        KeyPreview = true;
        KeyDown += HandleKeyDown;

        currentShape = GetRandomShapeWithCenteredPosition();
        nextShape = ShapesHandler.GetRandomShape();
        UpdateNextShapePreview();
        UpdateScoreAndLevelLabels();

        pictureBoxBoard.Paint += DrawGame;

        timer = new Timer { Interval = 500 };
        timer.Tick += UpdateGame;
        timer.Start();
    }

    private void HandleKeyDown(object? sender, KeyEventArgs e)
    {
        bool changed = e.KeyCode switch
        {
            Keys.Left => MoveShapeIfPossible(moveSide: -1),
            Keys.Right => MoveShapeIfPossible(moveSide: 1),
            Keys.Down => MoveShapeIfPossible(moveDown: 1),
            Keys.Up => TryRotateCurrentShape(),
            _ => false
        };

        if (changed)
        {
            pictureBoxBoard.Invalidate();
        }
    }

    private void UpdateGame(object? sender, EventArgs e)
    {
        bool moved = MoveShapeIfPossible(moveDown: 1);

        if (!moved)
        {
            UpdateGridWithCurrentShape();

            if (currentY < 0)
            {
                timer.Stop();
                MessageBox.Show("Game Over");
                Application.Restart();
            }

            ClearFilledRowsAndUpdateScore();
            currentShape = nextShape;
            nextShape = ShapesHandler.GetRandomShape();
            SetCurrentShapeStartPosition();
            UpdateNextShapePreview();
        }

        pictureBoxBoard.Invalidate();
    }

    private void DrawGame(object? sender, PaintEventArgs e)
    {
        Graphics graphics = e.Graphics;

        for (int x = 0; x < GridWidth; x++)
        {
            for (int y = 0; y < GridHeight; y++)
            {
                Rectangle cell = new Rectangle(x * CellSize, y * CellSize, CellSize, CellSize);
                Brush brush = grid[x, y] == 0 ? Brushes.Black : Brushes.Blue;
                graphics.FillRectangle(brush, cell);
                graphics.DrawRectangle(Pens.White, cell);
            }
        }

        for (int i = 0; i < currentShape.Width; i++)
        {
            for (int j = 0; j < currentShape.Height; j++)
            {
                int x = currentX + i;
                int y = currentY + j;

                if (currentShape.Cells[j, i] == 1
                    && x >= 0 && x < GridWidth
                    && y >= 0 && y < GridHeight)
                {
                    Rectangle cell = new Rectangle(x * CellSize, y * CellSize, CellSize, CellSize);
                    graphics.FillRectangle(Brushes.Blue, cell);
                    graphics.DrawRectangle(Pens.White, cell);
                }
            }
        }
    }

    private bool MoveShapeIfPossible(int moveDown = 0, int moveSide = 0)
    {
        int newX = currentX + moveSide;
        int newY = currentY + moveDown;

        if (newX < 0
            || newX + currentShape.Width > GridWidth
            || newY + currentShape.Height > GridHeight)
        {
            return false;
        }

        for (int i = 0; i < currentShape.Width; i++)
        {
            for (int j = 0; j < currentShape.Height; j++)
            {
                if (currentShape.Cells[j, i] == 1
                    && newY + j >= 0
                    && grid[newX + i, newY + j] == 1)
                {
                    return false;
                }
            }
        }

        currentX = newX;
        currentY = newY;
        return true;
    }

    private bool TryRotateCurrentShape()
    {
        int[,] rotatedCells = new int[currentShape.Width, currentShape.Height];

        for (int y = 0; y < currentShape.Height; y++)
        {
            for (int x = 0; x < currentShape.Width; x++)
            {
                rotatedCells[x, currentShape.Height - 1 - y] = currentShape.Cells[y, x];
            }
        }

        Shape rotatedShape = new Shape
        {
            Width = currentShape.Height,
            Height = currentShape.Width,
            Cells = rotatedCells
        };

        if (currentX + rotatedShape.Width > GridWidth
            || currentY + rotatedShape.Height > GridHeight)
        {
            return false;
        }

        for (int i = 0; i < rotatedShape.Width; i++)
        {
            for (int j = 0; j < rotatedShape.Height; j++)
            {
                if (rotatedShape.Cells[j, i] == 1
                    && currentY + j >= 0
                    && grid[currentX + i, currentY + j] == 1)
                {
                    return false;
                }
            }
        }

        currentShape = rotatedShape;
        return true;
    }

    private void UpdateGridWithCurrentShape()
    {
        for (int i = 0; i < currentShape.Width; i++)
        {
            for (int j = 0; j < currentShape.Height; j++)
            {
                int x = currentX + i;
                int y = currentY + j;

                if (currentShape.Cells[j, i] == 1
                    && x >= 0 && x < GridWidth
                    && y >= 0 && y < GridHeight)
                {
                    grid[x, y] = 1;
                }
            }
        }
    }

    private void ClearFilledRowsAndUpdateScore()
    {
        for (int y = GridHeight - 1; y >= 0; y--)
        {
            bool rowIsFull = true;

            for (int x = 0; x < GridWidth; x++)
            {
                if (grid[x, y] == 0)
                {
                    rowIsFull = false;
                    break;
                }
            }

            if (!rowIsFull)
            {
                continue;
            }

            score++;

            for (int row = y; row > 0; row--)
            {
                for (int x = 0; x < GridWidth; x++)
                {
                    grid[x, row] = grid[x, row - 1];
                }
            }

            for (int x = 0; x < GridWidth; x++)
            {
                grid[x, 0] = 0;
            }

            y++;
        }

        timer.Interval = Math.Max(50, 500 - score * 10);
        UpdateScoreAndLevelLabels();
    }

    private void UpdateScoreAndLevelLabels()
    {
        scoreLabel.Text = $"Score: {score}";
        levelLabel.Text = $"Level: {score / 10}";
    }

    private void UpdateNextShapePreview()
    {
        Bitmap preview = new Bitmap(6 * CellSize, 6 * CellSize);

        using (Graphics graphics = Graphics.FromImage(preview))
        {
            graphics.Clear(Color.Black);

            int startX = (6 - nextShape.Width) / 2;
            int startY = (6 - nextShape.Height) / 2;

            for (int i = 0; i < nextShape.Width; i++)
            {
                for (int j = 0; j < nextShape.Height; j++)
                {
                    if (nextShape.Cells[j, i] == 1)
                    {
                        Rectangle cell = new Rectangle(
                            (startX + i) * CellSize,
                            (startY + j) * CellSize,
                            CellSize,
                            CellSize);

                        graphics.FillRectangle(Brushes.Blue, cell);
                        graphics.DrawRectangle(Pens.White, cell);
                    }
                }
            }
        }

        Image? oldPreview = pictureBoxNext.Image;
        pictureBoxNext.Image = preview;
        oldPreview?.Dispose();
    }

    private Shape GetRandomShapeWithCenteredPosition()
    {
        Shape shape = ShapesHandler.GetRandomShape();
        currentShape = shape;
        SetCurrentShapeStartPosition();
        return shape;
    }

    private void SetCurrentShapeStartPosition()
    {
        currentX = (GridWidth - currentShape.Width) / 2;
        currentY = -currentShape.Height;
    }

    public class Shape
    {
        public int Width;
        public int Height;
        public int[,] Cells = new int[0, 0];
    }

    public static class ShapesHandler
    {
        private static readonly Random random = new Random();

        private static readonly Shape[] shapesArray =
        {
            new Shape { Width = 2, Height = 2, Cells = new[,] { { 1, 1 }, { 1, 1 } } },
            new Shape { Width = 1, Height = 4, Cells = new[,] { { 1 }, { 1 }, { 1 }, { 1 } } },
            new Shape { Width = 3, Height = 2, Cells = new[,] { { 0, 1, 0 }, { 1, 1, 1 } } },
            new Shape { Width = 3, Height = 2, Cells = new[,] { { 0, 0, 1 }, { 1, 1, 1 } } },
            new Shape { Width = 3, Height = 2, Cells = new[,] { { 1, 0, 0 }, { 1, 1, 1 } } },
            new Shape { Width = 3, Height = 2, Cells = new[,] { { 1, 1, 0 }, { 0, 1, 1 } } },
            new Shape { Width = 3, Height = 2, Cells = new[,] { { 0, 1, 1 }, { 1, 1, 0 } } }
        };

        public static Shape GetRandomShape()
        {
            return shapesArray[random.Next(shapesArray.Length)];
        }
    }
}
