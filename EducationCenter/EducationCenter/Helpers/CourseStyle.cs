namespace EducationCenter.Helpers;

public static class CourseStyle
{
    public static readonly string[] Categories =
        { "Languages", "Technology", "Science & Math", "Business", "Arts & Design", "Music" };

    public static readonly string[] Statuses = { "Open", "In Progress", "Completed", "Cancelled" };

    public static string StatusClass(string? s) => s switch
    {
        "Open" => "#10b981",
        "In Progress" => "#4f46e5",
        "Completed" => "#64748b",
        "Cancelled" => "#ef4444",
        _ => "#64748b"
    };

    public static readonly string[] Levels = { "Beginner", "Intermediate", "Advanced" };

    public static string Icon(string? c) => c switch
    {
        "Languages" => "bi-translate",
        "Technology" => "bi-code-slash",
        "Science & Math" => "bi-calculator",
        "Business" => "bi-briefcase",
        "Arts & Design" => "bi-palette",
        "Music" => "bi-music-note-beamed",
        _ => "bi-book"
    };

    public static string Color(string? c) => c switch
    {
        "Languages" => "#6366f1",
        "Technology" => "#0ea5e9",
        "Science & Math" => "#10b981",
        "Business" => "#f59e0b",
        "Arts & Design" => "#ec4899",
        "Music" => "#8b5cf6",
        _ => "#64748b"
    };

    public static string Initials(string? name)
    {
        var parts = (name ?? "?").Split(' ', StringSplitOptions.RemoveEmptyEntries);
        return parts.Length == 0 ? "?" :
               parts.Length == 1 ? parts[0][..1].ToUpper() :
               (parts[0][..1] + parts[^1][..1]).ToUpper();
    }

    private static readonly string[] Palette =
        { "#6366f1", "#0ea5e9", "#10b981", "#f59e0b", "#ec4899", "#8b5cf6", "#ef4444", "#14b8a6" };

    public static string AvatarColor(string? name)
    {
        int sum = 0;
        foreach (var ch in name ?? "") sum += ch;
        return Palette[sum % Palette.Length];
    }
}
