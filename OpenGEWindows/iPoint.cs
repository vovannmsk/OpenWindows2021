namespace OpenGEWindows
{
    public interface iPoint
    {
        void Move();
        void PressMouseR();  
        void PressMouseL();
        void PressMouseLL();
        void PressMouse();
        void PressMouseWheelUp();
        void PressMouseWheelDown();
        void Pause(int ms);  //
        void DoubleClickL();
        void Drag(iPoint point);
        void Turn(iPoint point);
        int getX();
        int getY();
    }
}
