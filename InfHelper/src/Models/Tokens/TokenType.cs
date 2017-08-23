namespace InfHelper.Models.Tokens
{
    public enum TokenType
    {
        // a-z A-Z
        Letter,
        // =
        EQ,
        // [
        CategoryOpening,
        // ]
        CategoryClosing,
        // \r space etc
        WhiteSpace,        
        // \n
        NewLine,
        // \
        LineConcatenator,
        // ,
        ValueSeparator,
        // "
        ValueMarker,
        // ;
        InlineComment,
    }
}