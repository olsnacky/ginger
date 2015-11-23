using GingerParser;
using GingerParser.CFG;
using GingerParser.DDG;
using GingerParser.Scope;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;

namespace WebApi.Controllers
{
    public class LintController : ApiController
    {
        public IEnumerable<string> Post([FromBody]string source)
        {
            Parser parser = new Parser(source);
            parser.parse();
            CFGVisitor cfgv = new CFGVisitor(parser.ast);
            ScopeVisitor sv = new ScopeVisitor(parser.ast);
            DDGVisitor ddgv = new DDGVisitor(parser.ast);
        }
    }
}
