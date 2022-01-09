using System;
using System.Drawing;
using System.Linq;
using System.Windows.Forms;

namespace GraphicsProgrammingAssignment
{
    /// <summary>
    /// Declearing variables for Bitmap,Graphics and pictureBox
    /// </summary>
    public class CommandParser
    {
        Bitmap bitmap;
        Graphics g;
        PictureBox pictureBox;

        /// <summary>
        /// The commandParser method creates a blank bitmap the size as the original
        /// and gets a graphics object from the new image
        /// </summary>
        /// <param name="pictureBox">To refer to the fields of the current class the PictureBox is 
        /// used to display graphics from a bitmap</param>
        public CommandParser(PictureBox pictureBox)
        {
            bitmap = new Bitmap(pictureBox.Size.Width, pictureBox.Size.Height);
            g = Graphics.FromImage(bitmap);
            this.pictureBox = pictureBox;
            draw();
        }
        /// <summary>
        /// this method trys to use Refresh method to update the picturebox the user draws.
        /// </summary>
        public void draw()
        {
            pictureBox.Image = bitmap;
            pictureBox.Refresh();
        }
        /// <summary> this method shows a  ParsePoint method  whereby the coordinates entered by 
        /// the user are seperated by a comma  using split function if the lenght of points is not
        /// equals to 2 it throws an exception if its less
        /// </summary>
        /// <param name="point">A string containing a number to convert.</param>
        /// <returns>The value of x and y coordinates</returns>
        /// <exception cref="Exception">Thrown when one parameter is not equals to 2</exception>
        public (int, int) ParsePoint(string point)
        {
            string[] points = point.Split(',');
            if (points.Length != 2)
            {
                throw new Exception();
            }

            if (int.TryParse(points[0], out int x) && int.TryParse(points[1], out int y))
            {
                return (x, y);
            }

            else
            {
                throw new Exception();
            }
        }
        /// <summary>
        /// 
        /// </summary>
        /// <param name="point"></param>
        /// <returns></returns>
        public Point ParseTriangle(string point)
        {
            var (x, y) = ParsePoint(point);
            return new Point(x, y);
        }
        /// <summary>
        /// A method,that run the users command which is  separated by a newline.
        /// function to execute a command enterd by the user contained within a string
        /// </summary>
        /// <param name="input"></param>
        /// <param name="userInput">A string containing a number to convert.</param>
        public void display(string input) {
            string[] inputArray = input.Split(new[] { "\r\n\r\n" }, StringSplitOptions.RemoveEmptyEntries);
                //System.Text.RegularExpressions.Regex.Split(input, @"\s{2,}");
           if(inputArray.Length == 2)
            {
                parseCommand(inputArray[0]);
                displayLoop(inputArray[1]);
            }
            else
            {

            }       
        }
        public void displayLoop(string input)
        {
            string[] userInputArray = input.Split(new string[] { Environment.NewLine },
                StringSplitOptions.RemoveEmptyEntries);
            Shapes draw = new Shapes();
            draw.x = 0;//initial x-coordinate axis 
            draw.y = 0;//initial y-coordinate axis
            draw.color = Pens.Black; //sets default color black 
            string globalNum = string.Empty; ;
            for (int i = 0; i < userInputArray.Length; i++)
            {
               
                // string[] commandParts = userInputArray[i].Split(' ');
                string[] commandParts = userInputArray[i].Split(' ');
                if (commandParts.Contains("="))
                {

                    continue;
                }
                if (commandParts.Contains("while"))
                {
                    continue;
                }
                {
                    switch (commandParts[0].ToLower())
                    {
                        case "circle":
                            if (commandParts.Length == 3)
                            {

                                // Create a position for the circle TryParse coordinates of points using a helper function.
                                (int, int) point = ParsePoint(commandParts[1]);
                                if (int.TryParse(commandParts[2], out int radius))
                                {
                                    new Circle(draw).DrawCircle(g, radius);
                                }

                                else
                                {
                                    // therefore if the radius of circle isnt parsed it throws an invalid commmand exception
                                    throw new Exception("Invalid Command Entered, Enter a Valid Command");
                                }
                            }
                            // if the shape has just two arguments the position is not
                            // included and the current position should be used instead

                            else if (commandParts.Length == 2)
                            {
                                var num = 0;

                                string[] previousCommand = userInputArray[i - 2].Split('=');
                                if (previousCommand.Length == 2)
                                {
                                    var str = previousCommand[1];

                                    if (!str.Contains("+"))
                                    {
                                        globalNum = str;
                                    }
                                    else
                                    {
                                        string[] splitSign = str.Split('+');
                                        str = (int.Parse(globalNum) + int.Parse(splitSign[1])).ToString();
                                    }
                                    if (int.TryParse(str, out int result))
                                    {
                                        num = result;
                                        while (num < 250)
                                        {
                                            new Circle(draw).DrawCircle(g, num);
                                            num++;
                                        }
                                        
                                        

                                    }
                                    else
                                    {
                                        MessageBox.Show("Invalid Command Entered, Enter a Valid Command");
                                    }
                                }
                                else if (previousCommand.Length == 3)
                                {

                                }
                                else
                                {
                                    MessageBox.Show("Invalid Command Entered, Enter a Valid Command");
                                }
                            }
                            else
                            {
                                MessageBox.Show("Invalid parameter Entered, Enter a Valid parameter");
                            }
                            break;
                        //   the shape triangle has three points therefore it needs three arguments parsed
                        case "triangle":
                            if (commandParts.Length == 4)
                            {
                                // three points are parsed using the helper function using The try catch statement which consists of a try
                                // which call drawTriangle if the points are 3 
                                // followed by one  catch clauses and else, which specifies for a different exception.
                                try
                                {
                                    Point point1 = ParseTriangle(commandParts[1]);
                                    Point point2 = ParseTriangle(commandParts[2]);
                                    Point point3 = ParseTriangle(commandParts[3]);
                                    new Triangle(draw).drawTriangle(g, point1, point2, point3);
                                }
                                catch
                                {
                                    MessageBox.Show("Invalid coordinate Entered, Enter a Valid coordinates");
                                }
                            }
                            else
                            {
                                MessageBox.Show("Invalid parameter Entered,Enter a valid parameter");
                            }
                            break;

                        case "rectangle":
                            // rectangle can have either three or two arguments
                            if (commandParts.Length == 3)
                            {
                                // parse the width and height and throw an exception if they are invalid 
                                // Create a position for the rectangle TryParse coordinates of points using a helper function.
                                if (int.TryParse(commandParts[1], out int x) && int.TryParse(commandParts[2], out int y))
                                    new Rectangle(draw).drawRectangle(g, x, y);

                                else
                                    MessageBox.Show("invalid parameter");
                            }
                            else if (commandParts.Length == 2)
                            {
                                try
                                {
                                    var (x, y) = ParsePoint(commandParts[1]);
                                    new Rectangle(draw).drawRectangle(g, x, y);
                                }
                                catch
                                {
                                    MessageBox.Show("Invalid coordinate  enter a valid coordinate");
                                }
                            }
                            else
                            {
                                MessageBox.Show("invalid parameter enter a valid parameter");
                            }
                            break;
                        // this command that takes an argument for the position for draw to
                        case "drawto":
                            if (commandParts.Length == 3)
                            {
                                if (int.TryParse(commandParts[1], out int x) && int.TryParse(commandParts[2], out int y))
                                    new Line(draw).drawLine(g, x, y);



                                else
                                    MessageBox.Show("Invalid parameter, Enter a Valid Parameter");
                            }
                            else if (commandParts.Length == 2)
                            {
                                try
                                {
                                    var (x, y) = ParsePoint(commandParts[1]);
                                    new Line(draw).drawLine(g, x, y);
                                }
                                catch
                                {
                                    MessageBox.Show("Invalid coordinate, Enter a Valid coordinate");
                                }
                            }
                            else
                            {
                                MessageBox.Show("invalid parameter, Enter a Valid Parameter");
                            }

                            break;

                        case "moveto":
                            //Moves the shape to the specified coordinates on the screen.
                            //by parsing the point using the helper function and sets the position to the users moveto input
                            try
                            {
                                (draw.x, draw.y) = ParsePoint(commandParts[1]);
                            }
                            catch
                            {
                                MessageBox.Show("invalid coordinate, Enter valid coordinates");
                            }
                            break;
                        case "fill":
                            switch (commandParts[1].ToLower())
                            {
                                case "on":  // case Fill shape is on it fills the shape
                                    draw.fill = true;
                                    break;
                                case "off": // case Fill shape is off 
                                    draw.fill = false;
                                    break;
                            }
                            break;

                        case "pen":
                            switch (commandParts[1].ToLower())
                            {
                                //Sets the Pen object to a defined set of color
                                case "yellow":
                                    draw.color = Pens.Yellow;
                                    break;
                                case "red":
                                    draw.color = Pens.Red;
                                    break;
                                case "black":
                                    draw.color = Pens.Black;
                                    break;
                                case "blue":
                                    draw.color = Pens.Blue;
                                    break;
                                default:
                                    draw.color = Pens.Black;
                                    break;
                            }
                            break;
                        case "clear":
                            // Clears screen with white background.
                            g.Clear(System.Drawing.Color.White);
                            break;
                        case "reset":
                            // Resets the location of the coordinates to its initial coordinate.
                            draw.x = 0;
                            draw.y = 0;
                            draw.color = Pens.Black;
                            break;
                        default:
                            MessageBox.Show("invalid Parameter Entered, enter a valid Parameter");
                            break;
                    }
                }
                this.draw();
            }
        }
        public void parseCommand(string userInput)
        {
            //Split  method on spaces.
            string[] userInputArray = userInput.Split(new string[] { Environment.NewLine },
                StringSplitOptions.RemoveEmptyEntries);
            Shapes draw = new Shapes();
            draw.x = 0;//initial x-coordinate axis 
            draw.y = 0;//initial y-coordinate axis
            draw.color = Pens.Black; //sets default color black 
            string globalNum = string.Empty; ;
            for (int i = 0; i < userInputArray.Length; i++)
            {
                if ((userInputArray.Length % 2) != 0)
                {
                    MessageBox.Show("Invalid parameter Entered, Enter a Valid parameter");
                    break;
                }
                // string[] commandParts = userInputArray[i].Split(' ');
                string[] commandParts = userInputArray[i].Split(' ');
                if (commandParts.Contains("="))
                {
             
                    continue;
                }
                switch (commandParts[0].ToLower())
                {
                    case "circle":
                        if (commandParts.Length == 3)
                        {
                            
                            // Create a position for the circle TryParse coordinates of points using a helper function.
                            (int, int) point = ParsePoint(commandParts[1]);
                            if (int.TryParse(commandParts[2], out int radius))
                            {
                                new Circle(draw).DrawCircle(g, radius);
                            }

                            else
                            {
                                // therefore if the radius of circle isnt parsed it throws an invalid commmand exception
                                throw new Exception("Invalid Command Entered, Enter a Valid Command");
                            }
                        }
                        // if the shape has just two arguments the position is not
                        // included and the current position should be used instead

                        else if (commandParts.Length == 2)
                        {
                            var num = 0;
                            string[] previousCommand = userInputArray[i - 1].Split('=');
                            if(previousCommand.Length == 2)
                            {
                               var str = previousCommand[1];

                                if (!str.Contains("+"))
                                {
                                    globalNum = str;
                                }
                                else
                                {
                                    string[] splitSign = str.Split('+');
                                    str = (int.Parse(globalNum) + int.Parse(splitSign[1])).ToString();
                                } 
                                if (int.TryParse(str, out int result))
                                {
                                    num = result;
                                    new Circle(draw).DrawCircle(g, num);
                                }
                                else
                                {
                                    MessageBox.Show("Invalid Command Entered, Enter a Valid Command");
                                }
                            }else if(previousCommand.Length == 3)
                            {

                            }
                            else
                            {
                                MessageBox.Show("Invalid Command Entered, Enter a Valid Command");
                            }
                        }
                        else
                        {
                            MessageBox.Show("Invalid parameter Entered, Enter a Valid parameter");
                        }
                        break;
                    //   the shape triangle has three points therefore it needs three arguments parsed
                    case "triangle":
                        if (commandParts.Length == 4)
                        {
                            // three points are parsed using the helper function using The try catch statement which consists of a try
                            // which call drawTriangle if the points are 3 
                            // followed by one  catch clauses and else, which specifies for a different exception.
                            try
                            {
                                Point point1 = ParseTriangle(commandParts[1]);
                                Point point2 = ParseTriangle(commandParts[2]);
                                Point point3 = ParseTriangle(commandParts[3]);
                                new Triangle(draw).drawTriangle(g, point1, point2, point3);
                            }
                            catch
                            {
                                MessageBox.Show("Invalid coordinate Entered, Enter a Valid coordinates");
                            }
                        }
                        else
                        {
                            MessageBox.Show("Invalid parameter Entered,Enter a valid parameter");
                        }
                        break;

                    case "rectangle":
                        // rectangle can have either three or two arguments
                        if (commandParts.Length == 3)
                        {
                            // parse the width and height and throw an exception if they are invalid 
                            // Create a position for the rectangle TryParse coordinates of points using a helper function.
                            if (int.TryParse(commandParts[1], out int x) && int.TryParse(commandParts[2], out int y))
                                new Rectangle(draw).drawRectangle(g, x, y);

                            else
                                MessageBox.Show("invalid parameter");
                        }
                        else if (commandParts.Length == 2)
                        {
                            try
                            {
                                var (x, y) = ParsePoint(commandParts[1]);
                                new Rectangle(draw).drawRectangle(g, x, y);
                            }
                            catch
                            {
                                MessageBox.Show("Invalid coordinate  enter a valid coordinate");
                            }
                        }
                        else
                        {
                            MessageBox.Show("invalid parameter enter a valid parameter");
                        }
                        break;
                    // this command that takes an argument for the position for draw to
                    case "drawto":
                        if (commandParts.Length == 3)
                        {
                            if (int.TryParse(commandParts[1], out int x) && int.TryParse(commandParts[2], out int y))
                                new Line(draw).drawLine(g, x, y);



                            else
                                MessageBox.Show("Invalid parameter, Enter a Valid Parameter");
                        }
                        else if (commandParts.Length == 2)
                        {
                            try
                            {
                                var (x, y) = ParsePoint(commandParts[1]);
                                new Line(draw).drawLine(g, x, y);
                            }
                            catch
                            {
                                MessageBox.Show("Invalid coordinate, Enter a Valid coordinate");
                            }
                        }
                        else
                        {
                            MessageBox.Show("invalid parameter, Enter a Valid Parameter");
                        }

                        break;

                    case "moveto":
                        //Moves the shape to the specified coordinates on the screen.
                        //by parsing the point using the helper function and sets the position to the users moveto input
                        try
                        {
                            (draw.x, draw.y) = ParsePoint(commandParts[1]);
                        }
                        catch
                        {
                            MessageBox.Show("invalid coordinate, Enter valid coordinates");
                        }
                        break;
                    case "fill":
                        switch (commandParts[1].ToLower())
                        {
                            case "on":  // case Fill shape is on it fills the shape
                                draw.fill = true;
                                break;
                            case "off": // case Fill shape is off 
                                draw.fill = false;
                                break;
                        }
                break;  
                
                    case "pen":
                        switch (commandParts[1].ToLower())
                        {
                            //Sets the Pen object to a defined set of color
                            case "yellow":
                                draw.color = Pens.Yellow;
                                break;
                            case "red":
                                draw.color = Pens.Red;
                                break;
                            case "black":
                                draw.color = Pens.Black;
                                break;
                            case "blue":
                                draw.color = Pens.Blue;
                                break;
                            default:
                                draw.color = Pens.Black;
                                break;
                        }
                        break;
                    case "clear":
                        // Clears screen with white background.
                        g.Clear(System.Drawing.Color.White);
                        break;
                    case "reset":
                        // Resets the location of the coordinates to its initial coordinate.
                        draw.x = 0;
                        draw.y = 0;
                        draw.color = Pens.Black;
                        break;
                    default:
                        MessageBox.Show("invalid Parameter Entered, enter a valid Parameter");
                        break;
                }
            }
            this.draw();
        }
    }
}
