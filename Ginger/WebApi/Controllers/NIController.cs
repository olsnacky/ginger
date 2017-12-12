using GingerParser;
using GingerParser.DFG;
using GingerParser.Scope;
using Newtonsoft.Json;
using System.Web.Http;
using System.Web.Http.Cors;

namespace WebApi.Controllers
{
    [EnableCors(origins: "http://localhost:4200, http://gngr.io, http://www.gngr.io, http://noilly.github.io", headers: "*", methods: "*")]
    public class NIController : ApiController
    {
        struct Result
        {
            bool _hasInterference;

            public bool hasInterference => _hasInterference;

            public Result(bool hasInterference)
            {
                _hasInterference = hasInterference;
            }
        }

        public IHttpActionResult Post([FromBody]string source)
        {
            if (source == null)
            {
                return BadRequest();
            }

            Parser parser = new Parser(source);
            parser.parse();
            //CFGVisitor cfgv = new CFGVisitor(parser.ast);
            ScopeVisitor sv = new ScopeVisitor(parser.ast);
            ClosureVisitor dfgv = new ClosureVisitor(parser.ast);
            //DDGVisitor ddgv = new DDGVisitor(parser.ast);
            bool result = dfgv.dfg.hasInterference();

            return Ok(new Result(result));
        }
    }
}
