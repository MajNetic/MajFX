# MajFX

A modern and customizable **WPF styling library** with built-in hover, click, and animation effects for `Button`, `ToggleButton`, and more.  
MajFX helps you build beautiful desktop applications faster, with smooth transitions and developer-friendly customization.  

🚀 Lightweight • 🎨 Beautiful • ⚡ Easy to use  

---

## ✨ Features
- Ready-to-use styles for `Button`, `ToggleButton`, and other controls  
- Smooth hover and click animations with customizable duration  
- Support for `Background` / `Foreground` bindings without losing original colors  
- Reusable resource dictionaries  
- Easy integration into any WPF project  

---

## 📦 Installation
1. Go to the [Releases](https://github.com/your-username/MajFX/releases) page.  
2. Download the latest **MajFX.dll** file.  
3. Add a reference to **MajFX.dll** in your WPF project.  
   - Right-click on your project → **Add Reference...** → Browse and select `MajFX.dll`.  
4. Merge the resource dictionary in your `App.xaml`:

```xml
<Application.Resources>
    <ResourceDictionary>
        <ResourceDictionary.MergedDictionaries>
            <ResourceDictionary Source="pack://application:,,,/MajFX;component/Themes/Generic.xaml"/>
        </ResourceDictionary.MergedDictionaries>
    </ResourceDictionary>
</Application.Resources>
```

🖥️ Usage Example

Here’s a quick example of how to use MajFX with a styled button:
```xml
<Button
    Style="{StaticResource MajFXButton}"
    Background="{Binding MyBackground}"
    Foreground="White"
    FontSize="18"
    maj:ButtonHelper.HoverBackground="Black"
    maj:ButtonHelper.HoverForeground="LightBlue"
    maj:ButtonHelper.ClickBackground="GreenYellow"
    maj:ButtonHelper.ClickForeground="Black"
    maj:ButtonHelper.CornerRadius="8"
    maj:ButtonHelper.HoverDuration="200"
    maj:ButtonHelper.ClickDuration="150">
    MajFX Button
</Button>