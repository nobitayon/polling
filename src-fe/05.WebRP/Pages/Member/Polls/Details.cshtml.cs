using Delta.Polling.Both.Member.Polls.Queries.GetPoll;
using Delta.Polling.FrontEnd.Logics.Member.Choices.Commands.AddAnotherChoiceOngoingPoll;
using Delta.Polling.FrontEnd.Logics.Member.Choices.Commands.AddChoices;
using Delta.Polling.FrontEnd.Logics.Member.Choices.Commands.DeleteChoice;
using Delta.Polling.FrontEnd.Logics.Member.Choices.Commands.EditChoice;
using Delta.Polling.FrontEnd.Logics.Member.Choices.Queries.GetChoice;
using Delta.Polling.FrontEnd.Logics.Member.Polls.Commands.AddVote;
using Delta.Polling.FrontEnd.Logics.Member.Polls.Commands.FinishPoll;
using Delta.Polling.FrontEnd.Logics.Member.Polls.Commands.StartPoll;
using Delta.Polling.FrontEnd.Logics.Member.Polls.Commands.UpdateVote;
using Delta.Polling.FrontEnd.Logics.Member.Polls.Queries.GetPoll;
using Microsoft.AspNetCore.Mvc.ViewFeatures;

namespace Delta.Polling.WebRP.Pages.Member.Polls;

public class DetailsModel : PageModelBase
{
    [BindProperty(SupportsGet = true)]
    public Guid PollId { get; init; }

    [BindProperty]
    public AddAnotherChoiceOngoingPollCommand Input { get; set; } = default!;

    [BindProperty]
    public AddChoiceCommand InputAddChoiceCommand { get; set; } = default!;

    [BindProperty]
    public StartPollCommand InputStartPollCommand { get; set; } = default!;

    [BindProperty]
    public AddVoteCommand InputAddVoteCommand { get; set; } = default!;

    [BindProperty]
    public UpdateVoteCommand InputUpdateVoteCommand { get; set; } = default!;

    public PollItem Poll { get; set; } = default!;

    public async Task<IActionResult> OnGet()
    {
        var response = await Sender.Send(new GetPollQuery { PollId = PollId });
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

    public async Task<IActionResult> OnPostAddAnotherChoice(AddAnotherChoiceOngoingPollCommand input)
    {
        if (ModelState.IsValid)
        {
            var response = await Sender.Send(input);

            if (response.Error is not null)
            {
                Error = response.Error;
                return Page();
            }

            if (response.Result is not null)
            {
                Console.WriteLine(response.Result.Data);
            }

            return new JsonResult(new { isValid = true });
        }
        else
        {
            return new JsonResult(new { isValid = false });
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
        var response = await Sender.Send(InputAddChoiceCommand);

        if (response.Error is not null)
        {
            Error = response.Error;
            return Page();
        }

        if (response.Result is not null)
        {
            TempData["success"] = "Success Add Choice";
            return RedirectToPage("/Member/Polls/Details", new { pollId = PollId });
        }
        else
        {
            TempData["failed"] = "Failed to Add Choice";
            return Page();
        }
    }

    public async Task<IActionResult> OnPostStartPollAsync(StartPollCommand command)
    {
        var response = await Sender.Send(command);

        if (response.Error is not null)
        {
            Error = response.Error;
            TempData["failed"] = Error.Detail;

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

        if (response.Error is not null)
        {
            Error = response.Error;
            TempData["failed"] = Error.Detail;

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

        Console.WriteLine("CEK");
        Console.WriteLine(convertedList.Count());
        InputUpdateVoteCommand.ListChoice = convertedList;
        var response = await Sender.Send(InputUpdateVoteCommand);

        if (response.Error is not null)
        {
            Error = response.Error;
            TempData["failed"] = Error.Detail;

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

            return Page();
        }
    }

    public async Task<IActionResult> OnGetEditChoice([FromRoute] Guid pollId, [FromQuery] Guid choiceId)
    {
        var input = new UpdateChoiceCommand { PollId = pollId, ChoiceId = choiceId, Description = default! };

        var responseGetChoice = await Sender.Send(new GetChoiceQuery { ChoiceId = choiceId });

        if (responseGetChoice.Error is not null)
        {
            Error = responseGetChoice.Error;
            TempData["failed"] = Error.Detail;

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

        if (response.Error is not null)
        {
            Error = response.Error;
            TempData["failed"] = Error.Detail;

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

        if (response.Error is not null)
        {
            Error = response.Error;
            TempData["failed"] = Error.Detail;

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

        if (response.Error is not null)
        {
            Error = response.Error;
            TempData["failed"] = Error.Detail;

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
}
