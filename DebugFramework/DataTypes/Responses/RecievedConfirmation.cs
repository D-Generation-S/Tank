namespace DebugFramework.DataTypes.Responses
{
    public class RecievedConfirmation : BaseDataType
    {
        uint MessageIdForConfirmation { get; set; }

        public RecievedConfirmation()
        {
            MessageIdForConfirmation = 0;
        }
    }
}
