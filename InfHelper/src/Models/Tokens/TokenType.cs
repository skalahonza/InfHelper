namespace InfHelper.Models.Tokens
{
    public enum TokenType
    {
        // ;
        InlineComment,
        // =
        EQ,
        // [
        CategoryOpening,
        // ]
        CategoryClosing,
        // \r space etc
        WhiteSpace,
        // a-z A-Z
        Letter,
        // \n
        NewLine,
        // \
        LineConcatenator,
    }
}