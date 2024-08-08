using Delta.Polling.Both.Member.Choices.Commands.AddChoice;
using Delta.Polling.Both.Member.Polls.Queries.GetPoll;
using Delta.Polling.FrontEnd.Logics.Member.Choices.Commands.AddAnotherChoiceOngoingPoll;
using Delta.Polling.FrontEnd.Logics.Member.Choices.Commands.AddChoices;
using Delta.Polling.FrontEnd.Logics.Member.Choices.Commands.DeleteChoice;
using Delta.Polling.FrontEnd.Logics.Member.Choices.Commands.EditChoice;
using Delta.Polling.FrontEnd.Logics.Member.Choices.Queries.GetChoice;
using Delta.Polling.FrontEnd.Logics.Member.Polls.Commands.AddVote;
using Delta.Polling.FrontEnd.Logics.Member.Polls.Commands.DeletePoll;
using Delta.Polling.FrontEnd.Logics.Member.Polls.Commands.FinishPoll;
using Delta.Polling.FrontEnd.Logics.Member.Polls.Commands.StartPoll;
using Delta.Polling.FrontEnd.Logics.Member.Polls.Commands.UpdatePoll;
using Delta.Polling.FrontEnd.Logics.Member.Polls.Commands.UpdateVote;
using Delta.Polling.FrontEnd.Logics.Member.Polls.Queries.GetPoll;
using Microsoft.AspNetCore.Mvc.ViewFeatures;

namespace Delta.Polling.WebRP.Pages.Member.Polls;

public class DetailsModel : PageModelBase
{
    [BindProperty(SupportsGet = true)]
    public Guid PollId { get; init; }

    [BindProperty]
    public AddAnotherChoiceOngoingPollCommand InputAnotherChoiceOngoingPollCommand { get; set; } = default!;

    [BindProperty]
    public AddChoiceCommand InputAddChoiceCommand { get; set; } = default!;

    [BindProperty]
    public AddChoiceFormDTO InputAddChoiceWithMediaCommand { get; set; } = default!;

    [BindProperty]
    public StartPollCommand InputStartPollCommand { get; set; } = default!;

    [BindProperty]
    public AddVoteCommand InputAddVoteCommand { get; set; } = default!;

    [BindProperty]
    public UpdateVoteCommand InputUpdateVoteCommand { get; set; } = default!;

    [BindProperty]
    public AddChoiceFormDTO InputAddChoiceMediaCommand { get; set; } = default!;

    public PollItem Poll { get; set; } = default!;

    private async Task LoadData()
    {
        var response = await Sender.Send(new GetPollQuery { PollId = PollId });

        if (response.Problem is not null)
        {
            Problem = response.Problem;

            return;
        }

        if (response.Result is null)
        {
            TempData["failed"] = "failed get poll";
        }

        if (response.Result is not null)
        {
            Poll = response.Result.Data;
        }
    }

    public async Task<IActionResult> OnGet()
    {
        var response = await Sender.Send(new GetPollQuery { PollId = PollId });

        if (response.Problem is not null)
        {
            Problem = response.Problem;

            return Page();
        }

        if (response.Result is null)
        {
            TempData["failed"] = "failed get poll";
        }

        if (response.Result is not null)
        {
            Poll = response.Result.Data;
        }

        return Page();
    }

    public PartialViewResult OnGetAddAnotherChoice([FromQuery] Guid pollId)
    {
        var input = new AddAnotherChoiceOngoingPollCommand { PollId = pollId, Description = null! };

        return new PartialViewResult
        {
            ViewName = "~/Pages/Member/Polls/PartialCustom/_AddAnotherChoice.cshtml",
            ViewData = new ViewDataDictionary<AddAnotherChoiceOngoingPollCommand>(ViewData, input)
        };
    }

    public async Task<IActionResult> OnPostAddAnotherChoice()
    {
        var response = await Sender.Send(InputAnotherChoiceOngoingPollCommand);

        if (response.Problem is not null)
        {
            Problem = response.Problem;
            await LoadData();
            return Page();
        }

        if (response.Result is not null)
        {
            TempData["success"] = "Success Add Another Choice";
            return RedirectToPage("/Member/Polls/Details", new { pollId = PollId });
        }
        else
        {
            TempData["failed"] = "Failed to Add Another Choice";
            await LoadData();
            return Page();
        }
    }

    public PartialViewResult OnGetStartPoll([FromQuery] Guid pollId)
    {
        var input = new StartPollCommand { PollId = pollId };
        return new PartialViewResult
        {
            ViewName = "~/Pages/Member/Polls/PartialCustom/_StartPollModal.cshtml",
            ViewData = new ViewDataDictionary<StartPollCommand>(ViewData, input)
        };
    }

    public async Task<IActionResult> OnPostAddChoiceAsync()
    {
        var formData = await Request.ReadFormAsync();

        var description = (string?)formData["Description"];
        if (string.IsNullOrEmpty(description))
        {
            await LoadData();
            Problem = new ProblemDetails { Title = "Oops", Detail = "not valid poll id" };
            return Page();
        }

        var isValidPollId = Guid.TryParse(formData["PollId"], out var pollId);
        var isDropdownAddMediaString = bool.TryParse(formData["DropdownAddMedia"], out var dropdownAddMedia);

        if (!isValidPollId)
        {
            await LoadData();
            Problem = new ProblemDetails { Title = "Oops", Detail = "not valid poll id" };
            return Page();
        }

        if (!isDropdownAddMediaString)
        {
            await LoadData();
            Problem = new ProblemDetails { Title = "Oops", Detail = "not valid dropdown" };
            return Page();
        }

        var mediaItems = new List<AddChoiceMediaRequest>();
        var index = 0;

        if (dropdownAddMedia)
        {
            while (formData.ContainsKey($"MediaItems_{index}_Description"))
            {
                var file = formData.Files[$"MediaItems_{index}_File"];
                if (file is null)
                {
                    await LoadData();
                    Problem = new ProblemDetails { Title = "Oops", Detail = "file null" };
                    return Page();
                }

                var fileDescription = (string?)formData[$"MediaItems_{index}_Description"];
                if (string.IsNullOrEmpty(fileDescription))
                {
                    await LoadData();
                    Problem = new ProblemDetails { Title = "Oops", Detail = "file description null" };
                    return Page();
                }

                mediaItems.Add(new AddChoiceMediaRequest
                {
                    File = file,
                    Description = fileDescription
                });

                index += 1;
            }
        }

        Console.WriteLine("Sampai sini kah 65");
        var command = new AddChoiceCommand
        {
            Description = description,
            PollId = pollId,
            MediaRequest = mediaItems
        };

        var response = await Sender.Send(command);

        if (response.Problem is not null)
        {
            Problem = response.Problem;
        }

        if (response.Result is not null)
        {
            TempData["success"] = "Success Add Choice";
        }
        else
        {
            TempData["failed"] = "Failed to Add Choice";
        }

        await LoadData();

        return Page();
    }

    public async Task<IActionResult> OnPostStartPollAsync(StartPollCommand command)
    {
        var response = await Sender.Send(command);

        if (response.Problem is not null)
        {
            Problem = response.Problem;
            TempData["failed"] = Problem.Detail;

            var redirectUrl = Url.Page("/Member/Polls/Details", new { pollId = PollId });

            return new JsonResult(new { isValid = false, redirectUrl = redirectUrl });
        }

        if (response.Result is not null)
        {
            TempData["success"] = "Success Start Poll";

            var redirectUrl = Url.Page("/Member/Polls/Details", new { pollId = PollId });

            return new JsonResult(new { isValid = true, redirectUrl = redirectUrl });
        }
        else
        {
            TempData["failed"] = "Failed to Start Poll";

            var redirectUrl = Url.Page("/Member/Polls/Details", new { pollId = PollId });

            return new JsonResult(new { isValid = true, redirectUrl = redirectUrl });
        }
    }

    public async Task<IActionResult> OnPostAddVoteAsync(List<string> listChoices)
    {
        var convertedList = new List<Guid>();
        foreach (var choice in listChoices)
        {
            convertedList.Add(new Guid(choice));
        }

        InputAddVoteCommand.ListChoice = convertedList;

        var response = await Sender.Send(InputAddVoteCommand);

        if (response.Problem is not null)
        {
            Problem = response.Problem;
            TempData["failed"] = Problem.Detail;
            await LoadData();

            return Page();
        }

        if (response.Result is not null)
        {
            TempData["success"] = "Success Add Vote";

            return RedirectToPage("/Member/Polls/Details", new { pollId = PollId });
        }
        else
        {
            TempData["failed"] = "Failed to Vote Poll";
            await LoadData();

            return Page();
        }
    }

    public async Task<IActionResult> OnPostUpdateVoteAsync(List<string> listChoices)
    {
        var convertedList = new List<Guid>();
        foreach (var choice in listChoices)
        {
            convertedList.Add(new Guid(choice));
        }

        InputUpdateVoteCommand.ListChoice = convertedList;
        var response = await Sender.Send(InputUpdateVoteCommand);

        if (response.Problem is not null)
        {
            Problem = response.Problem;
            TempData["failed"] = Problem.Detail;
            await LoadData();

            return Page();
        }

        if (response.Result is not null)
        {
            TempData["success"] = "Success Update Vote";

            return RedirectToPage("/Member/Polls/Details", new { pollId = PollId });
        }
        else
        {
            TempData["failed"] = "Failed to Vote Poll";
            await LoadData();
            return Page();
        }
    }

    public async Task<IActionResult> OnGetEditChoice([FromRoute] Guid pollId, [FromQuery] Guid choiceId)
    {
        var input = new UpdateChoiceCommand { PollId = pollId, ChoiceId = choiceId, Description = default! };

        var responseGetChoice = await Sender.Send(new GetChoiceQuery { ChoiceId = choiceId });

        if (responseGetChoice.Problem is not null)
        {
            Problem = responseGetChoice.Problem;
            TempData["failed"] = Problem.Detail;

            var redirectUrl = Url.Page("/Member/Polls/Details", new { pollId = PollId });

            return new JsonResult(new { isValid = false, redirectUrl = redirectUrl });
        }

        if (responseGetChoice.Result is null)
        {
            TempData["failed"] = "Failed to Edit Choice";

            var redirectUrl = Url.Page("/Member/Polls/Details", new { pollId = PollId });

            return new JsonResult(new { isValid = true, redirectUrl = redirectUrl });
        }

        var choice = responseGetChoice.Result.Data;

        input.Description = choice.Description;

        return new PartialViewResult
        {
            ViewName = "~/Pages/Member/Polls/PartialCustom/_EditChoiceModal.cshtml",
            ViewData = new ViewDataDictionary<UpdateChoiceCommand>(ViewData, input)
        };
    }

    public async Task<IActionResult> OnPostEditChoiceAsync(UpdateChoiceCommand command)
    {
        //Console.WriteLine($"{command.ChoiceId} {command.PollId} {command.Description}");
        //var errors = ModelState.Values.SelectMany(v => v.Errors);
        //foreach (var err in errors)
        //{
        //    Console.WriteLine(err.ErrorMessage);
        //    Console.WriteLine(err.Exception);
        //}

        //ModelState.ClearValidationState(nameof(UpdateChoiceCommand));
        //if (!TryValidateModel(command, nameof(UpdateChoiceCommand)))
        //{
        //    errors = ModelState.Values.SelectMany(v => v.Errors);
        //    Console.WriteLine("Setelah");
        //    foreach (var err in errors)
        //    {
        //        Console.WriteLine(err.ErrorMessage);
        //        Console.WriteLine(err.Exception);
        //    }
        //}

        //if (!ModelState.IsValid)
        //{
        //    Console.WriteLine("Not valid");
        //    return new JsonResult(new { isValid = false });
        //}

        var response = await Sender.Send(command);

        if (response.Problem is not null)
        {
            Problem = response.Problem;
            TempData["failed"] = Problem.Detail;

            var redirectUrl = Url.Page("/Member/Polls/Details", new { pollId = PollId });

            return new JsonResult(new { isValid = false, redirectUrl = redirectUrl });
        }

        if (response.Result is not null)
        {
            TempData["success"] = "Success Edit Choice";

            var redirectUrl = Url.Page("/Member/Polls/Details", new { pollId = PollId });

            return new JsonResult(new { isValid = true, redirectUrl = redirectUrl });
        }
        else
        {
            TempData["failed"] = "Failed to Edit Choice";

            var redirectUrl = Url.Page("/Member/Polls/Details", new { pollId = PollId });

            return new JsonResult(new { isValid = true, redirectUrl = redirectUrl });
        }
    }

    public IActionResult OnGetDeleteChoice([FromQuery] Guid choiceId)
    {
        var input = new DeleteChoiceCommand { ChoiceId = choiceId };

        return new PartialViewResult
        {
            ViewName = "~/Pages/Member/Polls/PartialCustom/_DeleteChoiceModal.cshtml",
            ViewData = new ViewDataDictionary<DeleteChoiceCommand>(ViewData, input)
        };
    }

    public async Task<IActionResult> OnPostDeleteChoiceAsync(DeleteChoiceCommand command)
    {
        var response = await Sender.Send(command);

        if (response.Problem is not null)
        {
            Problem = response.Problem;
            TempData["failed"] = Problem.Detail;

            var redirectUrl = Url.Page("/Member/Polls/Details", new { pollId = PollId });

            return new JsonResult(new { isValid = false, redirectUrl = redirectUrl });
        }

        if (response.Result is not null)
        {
            TempData["success"] = "Success Delete Choice";

            var redirectUrl = Url.Page("/Member/Polls/Details", new { pollId = PollId });

            return new JsonResult(new { isValid = true, redirectUrl = redirectUrl });
        }
        else
        {
            TempData["failed"] = "Failed Delete Choice";

            var redirectUrl = Url.Page("/Member/Polls/Details", new { pollId = PollId });

            return new JsonResult(new { isValid = true, redirectUrl = redirectUrl });
        }
    }

    public IActionResult OnGetFinishPollAsync()
    {
        Console.WriteLine($"finish poll {PollId}");
        var input = new FinishPollCommand { PollId = PollId };

        return new PartialViewResult
        {
            ViewName = "~/Pages/Member/Polls/PartialCustom/_FinishPollModal.cshtml",
            ViewData = new ViewDataDictionary<FinishPollCommand>(ViewData, input)
        };
    }

    public async Task<IActionResult> OnPostFinishPollAsync(FinishPollCommand command)
    {
        var response = await Sender.Send(command);

        if (response.Problem is not null)
        {
            Problem = response.Problem;
            TempData["failed"] = Problem.Detail;

            var redirectUrl = Url.Page("/Member/Polls/Details", new { pollId = PollId });

            return new JsonResult(new { isValid = false, redirectUrl = redirectUrl });
        }

        if (response.Result is not null)
        {
            TempData["success"] = "Success Finish Poll";

            var redirectUrl = Url.Page("/Member/Polls/Details", new { pollId = PollId });

            return new JsonResult(new { isValid = true, redirectUrl = redirectUrl });
        }
        else
        {
            TempData["failed"] = "Failed to Start Poll";

            var redirectUrl = Url.Page("/Member/Polls/Details", new { pollId = PollId });

            return new JsonResult(new { isValid = true, redirectUrl = redirectUrl });
        }
    }

    public async Task<IActionResult> OnGetUpdatePollAsync()
    {
        var input = new UpdatePollCommand
        {
            PollId = PollId,
            Title = default!,
            Question = default!,
            MaximumAnswer = default!,
            AllowOtherChoice = default!
        };

        var responseGetPoll = await Sender.Send(new GetPollQuery { PollId = PollId });

        if (responseGetPoll.Problem is not null)
        {
            Problem = responseGetPoll.Problem;
            TempData["failed"] = Problem.Detail;

            var redirectUrl = Url.Page("/Member/Polls/Details", new { pollId = PollId });

            return new JsonResult(new { isValid = false, redirectUrl = redirectUrl });
        }

        if (responseGetPoll.Result is null)
        {
            TempData["failed"] = "Failed to Get Poll";

            var redirectUrl = Url.Page("/Member/Polls/Details", new { pollId = PollId });

            return new JsonResult(new { isValid = true, redirectUrl = redirectUrl });
        }

        var poll = responseGetPoll.Result.Data;

        input.Title = poll.Title;
        input.Question = poll.Question;
        input.MaximumAnswer = poll.MaximumAnswer;
        input.AllowOtherChoice = poll.AllowOtherChoice;

        return new PartialViewResult
        {
            ViewName = "~/Pages/Member/Polls/PartialCustom/_UpdatePollModal.cshtml",
            ViewData = new ViewDataDictionary<UpdatePollCommand>(ViewData, input)
        };
    }

    public async Task<IActionResult> OnPostUpdatePollAsync(UpdatePollCommand command)
    {
        var response = await Sender.Send(command);

        if (response.Problem is not null)
        {
            Problem = response.Problem;
            TempData["failed"] = Problem.Detail;

            var redirectUrl = Url.Page("/Member/Polls/Details", new { pollId = PollId });

            return new JsonResult(new { isValid = false, redirectUrl = redirectUrl });
        }

        if (response.Result is not null)
        {
            TempData["success"] = "Success Update Poll";

            var redirectUrl = Url.Page("/Member/Polls/Details", new { pollId = PollId });

            return new JsonResult(new { isValid = true, redirectUrl = redirectUrl });
        }
        else
        {
            TempData["failed"] = "Failed to Update Poll";

            var redirectUrl = Url.Page("/Member/Polls/Details", new { pollId = PollId });

            return new JsonResult(new { isValid = true, redirectUrl = redirectUrl });
        }
    }

    public async Task<IActionResult> OnGetDeletePollAsync()
    {
        var input = new DeletePollCommand
        {
            PollId = PollId
        };

        var responseGetPoll = await Sender.Send(new GetPollQuery { PollId = PollId });

        if (responseGetPoll.Problem is not null)
        {
            Problem = responseGetPoll.Problem;
            TempData["failed"] = Problem.Detail;

            var redirectUrl = Url.Page("/Member/Polls/Details", new { pollId = PollId });

            return new JsonResult(new { isValid = false, redirectUrl = redirectUrl });
        }

        if (responseGetPoll.Result is null)
        {
            TempData["failed"] = "Failed to Get Poll";

            var redirectUrl = Url.Page("/Member/Polls/Details", new { pollId = PollId });

            return new JsonResult(new { isValid = true, redirectUrl = redirectUrl });
        }

        var poll = responseGetPoll.Result.Data;

        var viewData = new ViewDataDictionary(ViewData)
        {
            ["Input"] = input,
            ["AdditionalData"] = poll
        };

        return new PartialViewResult
        {
            ViewName = "~/Pages/Member/Polls/PartialCustom/_DeletePollModal.cshtml",
            ViewData = viewData
        };
    }

    public async Task<IActionResult> OnPostDeletePollAsync(DeletePollCommand command)
    {
        var response = await Sender.Send(command);

        if (response.Problem is not null)
        {
            Problem = response.Problem;
            TempData["failed"] = Problem.Detail;

            var redirectUrl = Url.Page("/Member/Polls/My");

            return new JsonResult(new { isValid = false, redirectUrl = redirectUrl });
        }

        if (response.Result is not null)
        {
            TempData["success"] = "Success Delete Poll";

            var redirectUrl = Url.Page("/Member/Polls/My");

            return new JsonResult(new { isValid = true, redirectUrl = redirectUrl });
        }
        else
        {
            TempData["failed"] = "Failed to Delete Poll";

            var redirectUrl = Url.Page("/Member/Polls/Details", new { pollId = PollId });

            return new JsonResult(new { isValid = true, redirectUrl = redirectUrl });
        }
    }

    public async Task<IActionResult> OnPostCheckSansAsync()
    {
        //Console.WriteLine("sini");
        //Console.WriteLine(command.Description);
        //Console.WriteLine(command.PollId);
        ////Console.WriteLine(command.FileInput.Count());
        //Console.WriteLine("sini");
        await LoadData();
        return Page();
    }
}

public record AddChoiceFormDTO
{
    public required string Description { get; init; }
    public required bool DropdownAddMedia { get; init; } = false;
    public required Guid PollId { get; init; } = Guid.Empty;
    public required AddChoiceMediaRequestTheFuck MediaRequest { get; init; } = default!;
}

public class AddChoiceMediaRequestTheFuck
{
    public List<MediaItem> MediaItems { get; set; } = [];
}

public record MediaItem
{
    public IFormFile File { get; init; } = default!;
    public required string Description { get; set; }
}
