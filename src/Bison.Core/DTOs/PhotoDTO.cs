public record PhotoDTO(
    string FileName,
    string ContentType,
    long Size,
    Stream Data);