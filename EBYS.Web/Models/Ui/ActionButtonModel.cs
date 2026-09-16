namespace EBYS.Web.Models.Ui;

public enum ActionButtonVariant
{
    Primary,
    Secondary,
    Danger
}

/// <summary>
/// Uygulamadaki işlem butonlarının ortak sözleşmesi.
/// Görsel kararlar bu modelde değil, design-system.css dosyasında tutulur.
/// </summary>
public class ActionButtonModel
{
    public string Text { get; init; } = string.Empty;
    public ActionButtonVariant Variant { get; init; } = ActionButtonVariant.Primary;
    public string? Id { get; init; }
    public string? IconClass { get; init; }
    public string? Href { get; init; }
    public string? OnClick { get; init; }
    public string Type { get; init; } = "button";
    public string? DataBsDismiss { get; init; }
    public bool Disabled { get; init; }
    public bool IsBlock { get; init; }

    public bool IsLink => !string.IsNullOrWhiteSpace(Href);
}
