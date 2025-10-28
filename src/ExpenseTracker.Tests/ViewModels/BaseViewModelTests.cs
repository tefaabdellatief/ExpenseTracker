using Xunit;
using CommunityToolkit.Mvvm.ComponentModel;

namespace ExpenseTracker.Tests.ViewModels;

public partial class BaseViewModelTests
{
    // Testable BaseViewModel implementation
    private partial class TestableBaseViewModel : ObservableObject
    {
        [ObservableProperty] 
        private bool isBusy;
        
        [ObservableProperty] 
        private string? errorMessage;

        public async Task TestRunSafeAsync(Func<Task> action)
        {
            await RunSafeAsync(action);
        }

        protected async Task RunSafeAsync(Func<Task> action)
        {
            try
            {
                IsBusy = true;
                await action();
            }
            catch (Exception ex)
            {
                ErrorMessage = ex.Message;
            }
            finally
            {
                IsBusy = false;
            }
        }
    }

    [Fact]
    public void Constructor_ShouldInitializeProperties()
    {
        // Arrange & Act
        var viewModel = new TestableBaseViewModel();

        // Assert
        Assert.False(viewModel.IsBusy);
        Assert.Null(viewModel.ErrorMessage);
    }

    [Fact]
    public void IsBusy_PropertyChanged_ShouldNotifyPropertyChanged()
    {
        // Arrange
        var viewModel = new TestableBaseViewModel();
        var propertyChangedRaised = false;
        
        viewModel.PropertyChanged += (sender, e) =>
        {
            if (e.PropertyName == nameof(viewModel.IsBusy))
                propertyChangedRaised = true;
        };

        // Act
        viewModel.IsBusy = true;

        // Assert
        Assert.True(propertyChangedRaised);
        Assert.True(viewModel.IsBusy);
    }

    [Fact]
    public void ErrorMessage_PropertyChanged_ShouldNotifyPropertyChanged()
    {
        // Arrange
        var viewModel = new TestableBaseViewModel();
        var propertyChangedRaised = false;
        
        viewModel.PropertyChanged += (sender, e) =>
        {
            if (e.PropertyName == nameof(viewModel.ErrorMessage))
                propertyChangedRaised = true;
        };

        // Act
        viewModel.ErrorMessage = "Test error";

        // Assert
        Assert.True(propertyChangedRaised);
        Assert.Equal("Test error", viewModel.ErrorMessage);
    }

    [Fact]
    public async Task RunSafeAsync_WithSuccessfulAction_ShouldSetIsBusyCorrectly()
    {
        // Arrange
        var viewModel = new TestableBaseViewModel();
        var actionExecuted = false;
        var busyStates = new List<bool>();

        viewModel.PropertyChanged += (sender, e) =>
        {
            if (e.PropertyName == nameof(viewModel.IsBusy))
                busyStates.Add(viewModel.IsBusy);
        };

        // Act
        await viewModel.TestRunSafeAsync(async () =>
        {
            await Task.Delay(10); // Simulate async work
            actionExecuted = true;
        });

        // Assert
        Assert.True(actionExecuted);
        Assert.False(viewModel.IsBusy); // Should be false after completion
        Assert.Contains(true, busyStates); // Should have been set to true during execution
        Assert.Null(viewModel.ErrorMessage);
    }

    [Fact]
    public async Task RunSafeAsync_WithException_ShouldHandleErrorCorrectly()
    {
        // Arrange
        var viewModel = new TestableBaseViewModel();
        var expectedException = new InvalidOperationException("Test exception");

        // Act
        await viewModel.TestRunSafeAsync(() => throw expectedException);

        // Assert
        Assert.False(viewModel.IsBusy); // Should be false after completion
        Assert.Equal("Test exception", viewModel.ErrorMessage);
    }

    [Fact]
    public async Task RunSafeAsync_WithAsyncException_ShouldHandleErrorCorrectly()
    {
        // Arrange
        var viewModel = new TestableBaseViewModel();
        var expectedException = new ArgumentException("Async test exception");

        // Act
        await viewModel.TestRunSafeAsync(async () =>
        {
            await Task.Delay(10);
            throw expectedException;
        });

        // Assert
        Assert.False(viewModel.IsBusy); // Should be false after completion
        Assert.Equal("Async test exception", viewModel.ErrorMessage);
    }

    //multi exception test to ensure error message is updated each time
    [Fact]
    public async Task RunSafeAsync_MultipleExceptions_ShouldUpdateErrorMessage()
    {
        // Arrange
        var viewModel = new TestableBaseViewModel();

        // Act - First exception
        await viewModel.TestRunSafeAsync(() => throw new Exception("First error"));
        var firstError = viewModel.ErrorMessage;

        // Act - Second exception
        await viewModel.TestRunSafeAsync(() => throw new Exception("Second error"));
        var secondError = viewModel.ErrorMessage;

        // Assert
        Assert.Equal("First error", firstError);
        Assert.Equal("Second error", secondError);
        Assert.False(viewModel.IsBusy);
    }

    //one successful and one failing to ensure IsBusy is reset in both cases
    [Fact]
    public async Task RunSafeAsync_ShouldAlwaysSetIsBusyToFalseInFinally()
    {
        // Arrange
        var viewModel = new TestableBaseViewModel();
        
        // Act - Test with successful action
        await viewModel.TestRunSafeAsync(async () => await Task.Delay(10));
        var busyAfterSuccess = viewModel.IsBusy;

        // Act - Test with exception
        await viewModel.TestRunSafeAsync(() => throw new Exception("Test"));
        var busyAfterException = viewModel.IsBusy;

        // Assert
        Assert.False(busyAfterSuccess);
        Assert.False(busyAfterException);
    }
}