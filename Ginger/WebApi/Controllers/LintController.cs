using GingerParser;
using GingerParser.CFG;
using GingerParser.DDG;
using GingerParser.Scope;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;
using System.Web.Http.Cors;

namespace WebApi.Controllers
{
    [EnableCors(origins: "http://127.0.0.1:8080, http://gngr.io, http://www.gngr.io, http://noilly.github.io", headers: "*", methods: "*")]
    public class LintController : ApiController
    {
        public struct ParseExceptionResult
        {
            public int row;
            public int column;
            public string reason;
            public string level;

            public ParseExceptionResult(int row, int col, string reason, string level)
            {
                this.row = row;
                this.column = col;
                this.reason = reason;
                this.level = level;
            }
        }

        public IHttpActionResult Post([FromBody]string source)
        {
            Debug.WriteLine(source);
            if (source == null) {
                return BadRequest();
            }

            List<ParseException> errors = new List<ParseException>();
            List<ParseExceptionResult> errorResults = new List<ParseExceptionResult>();

            Parser parser = new Parser(source);
            parser.parse();
            CFGVisitor cfgv = new CFGVisitor(parser.ast);
            ScopeVisitor sv = new ScopeVisitor(parser.ast);
            DDGVisitor ddgv = new DDGVisitor(parser.ast);

            errors.AddRange(parser.errors);
            errors.AddRange(sv.errors);
            errors.AddRange(ddgv.errors);

            foreach (ParseException error in errors)
            {
                errorResults.Add(new ParseExceptionResult(error.row, error.column, error.reason, error.level.ToString()));
                
            }

            return Ok(errorResults);
        }
    }
}
