using LoreCreator.Controllers.CustomAttributes;
using LoreCreator.DbStuff.Models;
using LoreCreator.DbStuff.Repositories;
using LoreCreator.Services;
using LoreCreator.ViewModels;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using SharedForLoreCreator.Models;
using System.Runtime.InteropServices;
using System.Text.Json;
using System.Text.Json.Nodes;

namespace LoreCreator.Controllers;

public class LoreCreatorController : Controller
{
    private TagRepository _tagRepository;
    private ElementRepository _elementRepository;
    private ConnectionRepository _connectionRepository;
    private ConnectionTypeRepository _connectionTypeRepository;
    private ChatServiceWorker _chatServiceWorker;

    private IWebHostEnvironment _webHostEnvironment;

    public LoreCreatorController(TagRepository tagRepository,
        ElementRepository elementRepository,
        ConnectionRepository connectionRepository,
        ConnectionTypeRepository connectionTypeRepository,
        ChatServiceWorker chatServiceWorker,
        IWebHostEnvironment webHostEnvironment)
    {
        _tagRepository = tagRepository;
        _elementRepository = elementRepository;
        _connectionRepository = connectionRepository;
        _connectionTypeRepository = connectionTypeRepository;
        _chatServiceWorker = chatServiceWorker;
        _webHostEnvironment = webHostEnvironment;
    }
    public IActionResult FunFact()
    {
        using (HttpClient client = new())
        {
            var jsonString = client.GetAsync("https://api.chucknorris.io/jokes/random").Result.Content.ReadAsStringAsync().Result;
            var json = JsonNode.Parse(jsonString);
            ViewBag.Result = json["value"].GetValue<string>();

        }
        return View();
    }

    public IActionResult Index()
    {

        IndexViewModel indexVM = new()
        {
            Elements = _elementRepository.GetAll().ToList()
        };

        return View(indexVM);
    }

    public IActionResult DELETEALL()
    {
        foreach (var connection in _connectionRepository.GetAll())
        {
            _connectionRepository.Delete(connection.Id);
        }
        foreach (var conType in _connectionTypeRepository.GetAll())
        {
            _connectionTypeRepository.Delete(conType.Id);
        }
        foreach (var element in _elementRepository.GetAll())
        {
            _elementRepository.Delete(element.Id);
            _chatServiceWorker.DeleteChat("element", element.Id);
        }
        foreach (var tag in _tagRepository.GetAll())
        {
            _tagRepository.Delete(tag.Id);
        }
        return RedirectToAction("Index");
    }

    [PermissionCheck(PermissionTypes.AddElement)]
    [Authorize]
    [HttpPost]
    public IActionResult UpdateElementImage(int id, IFormFile avatar)
    {
        UpdateElementImageDeep(id, avatar);
        return RedirectToAction("GetElement", new { id });
    }

    [PermissionCheck(PermissionTypes.AddElement)]
    [Authorize]
    [HttpPost]
    public IActionResult UpdateElementName(int id, string name)
    {
        _elementRepository.UpdateName(id, name);
        return RedirectToAction("GetElement", new { id });
    }

    [PermissionCheck(PermissionTypes.AddElement)]
    [Authorize]
    [HttpPost]
    public IActionResult UpdateElementDescription(int id, string description)
    {
        _elementRepository.UpdateDescription(id, description);
        return RedirectToAction("GetElement", new { id });
    }

    [NonAction]
    public void UpdateElementImageDeep(int id, IFormFile avatar)
    {
        // upload image
        var extension = Path.GetExtension(avatar.FileName);

        var fileName = $"elementImage{id}{extension}";
        var filePath = Path.Combine(_webHostEnvironment.WebRootPath, "images", "loreCreatorElementsImages", fileName);
        using (var fileStream = new FileStream(filePath, FileMode.Create))
        {
            avatar.CopyTo(fileStream);
        }

        var avatarUrl = $"/images/loreCreatorElementsImages/{fileName}";
        _elementRepository.UpdateAvatar(id, avatarUrl);
    }

    [PermissionCheck(PermissionTypes.AddElement)]
    [Authorize]
    [HttpGet]
    public IActionResult AddElement()
    {
        ViewBag.Tags = _tagRepository.GetAll().ToList();
        return View(new AddElementViewModel() { Tags = new List<int>()});
    }

    [PermissionCheck(PermissionTypes.AddElement)]
    [Authorize]
    [HttpPost]
    public IActionResult AddElement(AddElementViewModel addElementViewModel)
    {
        if (!ModelState.IsValid)
        {
            return View(addElementViewModel);
        }
        List<Tag> tags = new();
        foreach(var t in addElementViewModel.Tags)
        {
            if(_tagRepository.GetById(t) is Tag tag)
            {
                tags.Add(tag);
            }
        }
        int id = _elementRepository.Add(new()
        {
            Name = addElementViewModel.Name,
            Description = addElementViewModel.Description,
            Tags = tags,
            Image = addElementViewModel.Image
        });
        if (!_chatServiceWorker.AddChat("element", id).Result)
        {
            _elementRepository.Delete(id);
            return RedirectToAction("Index");
        }
        return RedirectToAction("GetElement", new {id = id});
    }
    [PermissionCheck(PermissionTypes.RemoveElement)]
    [Authorize]
    public IActionResult RemoveTagFromElement(int elementId, int tagId)
    {
        int id = elementId;
        if(!_elementRepository.isExist(elementId))
        {
            return RedirectToAction("GetElement", new { id });
        }
        if(!_tagRepository.isExist(tagId))
        {
            return RedirectToAction("GetElement", new { id });
        }
        Tag tag = _tagRepository.GetById(tagId);
        if(!_elementRepository.GetById(elementId).Tags.Contains(tag))
        {
            return RedirectToAction("GetElement", new { id });
        }
        _elementRepository.RemoveTag(elementId, tag);
        return RedirectToAction("GetElement", new { id });
    }

    [PermissionCheck(PermissionTypes.AddElement)]
    [Authorize]
    public IActionResult DeleteElement(int elementId)
    {
        if (!_elementRepository.isExist(elementId))
        {
            return RedirectToAction("GetElement", new { id = elementId });
        }
        _elementRepository.Delete(elementId);
        _chatServiceWorker.DeleteChat("element", elementId);
        return RedirectToAction("Index");
    }

    public IActionResult GetTag(int id)
    {
        if(!_tagRepository.isExist(id))
        {
            throw new Exception("Tag id is not exist");
        }
        return View(_tagRepository.GetById(id));
    }

    [PermissionCheck(PermissionTypes.RemoveTag)]
    [Authorize]
    public IActionResult DeleteTag(int tagId)
    {
        if (!_tagRepository.isExist(tagId))
        {
            return RedirectToAction("GetTag", new { id = tagId });
        }
        _tagRepository.Delete(tagId);
        _chatServiceWorker.DeleteChat("tag", tagId);
        return RedirectToAction("Index");
    }

    public IActionResult GetConnection(int id)
    {
        if(!_connectionRepository.isExist(id))
        {
            throw new Exception("Connection id is not exist");
        }
        return View(_connectionRepository.GetById(id));
    }

    [PermissionCheck(PermissionTypes.RemoveConnection)]
    [Authorize]
    public IActionResult DeleteConnection(int connectionId)
    {
        if (!_connectionRepository.isExist(connectionId))
        {
            return RedirectToAction("GetConnection", new { id = connectionId });
        }
        _connectionRepository.Delete(connectionId);
        _chatServiceWorker.DeleteChat("connection", connectionId);
        return RedirectToAction("Index");
    }

    public IActionResult GetElement(int id)
    {
        if(!_elementRepository.isExist(id))
        {
            throw new Exception("Element id is not exist");
        }
        Element element = _elementRepository.GetById(id);
        List<Connection> connections = new();
        foreach(var cons in element.Connections)
        {
            connections.Add(_connectionRepository.GetById(cons.Id));
        }
        var a = new GetElementViewModel()
        {
            Id = element.Id,
            Name = element.Name,
            Description = element.Description,
            Image = element.Image,
            Connections = connections,
            Tags = element.Tags
        };
        return View(a);
    }

    [Authorize]
    [PermissionCheck(PermissionTypes.AddTag)]
    [HttpPost]
    public IActionResult AddTag(string name, string description)
    {
        if (string.IsNullOrEmpty(name))
        {
            throw new Exception("Name is null or empty");
        }
        if(string.IsNullOrEmpty(description))
        {
            throw new Exception("Description is null or empty");
        }
        int id = _tagRepository.Add(new() { Name = name, Description = description });
        if (!_chatServiceWorker.AddChat("tag", id).Result)
        {
            _tagRepository.Delete(id);
            return RedirectToAction("Index");
        }

        return RedirectToAction("AddTagOrConnectionType");
    }

    [Authorize]
    [PermissionCheck(PermissionTypes.AddTag, PermissionTypes.AddConnectionType)]
    [HttpGet]
    public IActionResult AddTagOrConnectionType()
    {
        return View();
    }

    [PermissionCheck(PermissionTypes.AddConnectionType)]
    [Authorize]
    [HttpPost]
    public IActionResult AddConnectionType(string name)
    {
        if (string.IsNullOrEmpty(name))
        {
            throw new Exception("Name is null or empty");
        }
        int id = _connectionTypeRepository.Add(new() { Name = name });
        if (!_chatServiceWorker.AddChat("connectionType", id).Result)
        {
            _connectionTypeRepository.Delete(id);
            return RedirectToAction("Index");
        }

        return RedirectToAction("Index");
    }

    [PermissionCheck(PermissionTypes.RemoveConnectionType)]
    [Authorize]
    public IActionResult DeleteConnectionType(int connectionTypeId)
    {
        if (!_connectionRepository.isExist(connectionTypeId))
        {
            return RedirectToAction("GetConnectionType", new { id = connectionTypeId });
        }
        _connectionRepository.Delete(connectionTypeId);
        _chatServiceWorker.DeleteChat("connectionType", connectionTypeId);
        return RedirectToAction("Index");
    }

    [PermissionCheck(PermissionTypes.AddConnection)]
    [Authorize]
    [HttpGet]
    public IActionResult AddConnection()
    {
        return View(new AddConnectionViewModel()
        {
            AllConnectionTypes = _connectionTypeRepository.GetAll() as List<ConnectionType>,
            AllElements = _elementRepository.GetAll() as List<Element>
        });
    }

    [PermissionCheck(PermissionTypes.AddConnection)]
    [Authorize]
    [HttpPost]
    public IActionResult AddConnection(AddConnectionViewModel connection)
    {
        if(!_connectionTypeRepository.isExist(connection.ConnectionTypeId))
        {
            throw new Exception("Connection type id is not exist");
        }
        if(!connection.ElementsIds.All(e => _elementRepository.isExist(e)))
        {
            throw new Exception("Some element is not exist");
        }
        if(string.IsNullOrEmpty(connection.Description))
        {
            throw new Exception("Description is null or empty");
        }

        List<Element> elements = new();
        foreach(var elementId in connection.ElementsIds)
        {
            elements.Add(_elementRepository.GetById(elementId));
        }

        int id = _connectionRepository.Add(new()
        {
            ConnectionType = _connectionTypeRepository.GetById(connection.ConnectionTypeId),
            Elements = elements,
            Description = connection.Description
        });
        if (!_chatServiceWorker.AddChat("connection", id).Result)
        {
            _connectionRepository.Delete(id);
            return RedirectToAction("Index");
        }

        return RedirectToAction("Index");
    }
}
