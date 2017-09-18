
interface IState<T>
{
    T GetAction();
    Transition[] GetTransitions();
    string GetName();
}
