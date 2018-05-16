
/// <summary>
/// Static class that holds globally accessible data.
/// </summary>
public class ApplicationDataContainer
{
    /// <summary>
    /// Reference to loaded recording object.
    /// </summary>
    public static Recording loadedRecording;

    /// <summary>
    /// Reference to loaded experiment object.
    /// </summary>
    public static Experiment loadedExperiment;

    /// <summary>
    /// Determines whether the experiment is in replay mode.
    /// </summary>
    public static bool replay = false;

    /// <summary>
    /// Run mode of the app - VR or non VR
    /// </summary>
    public static Controller runMode;

    /// <summary>
    /// Prevents bad behaviour when returning back to the main menu.
    /// </summary>
    public static bool initialSetupDone = false;
}