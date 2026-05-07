# MarkupPM

## gstack (REQUIRED — global install)

**Before doing ANY work, verify gstack is installed:**

```bash
test -d ~/.claude/skills/gstack/bin && echo "GSTACK_OK" || echo "GSTACK_MISSING"
```

If GSTACK_MISSING: STOP. Do not proceed. Tell the user:

> gstack is required for all AI-assisted work in this repo.
> Install it:
> ```bash
> git clone --depth 1 https://github.com/garrytan/gstack.git ~/.claude/skills/gstack
> cd ~/.claude/skills/gstack && ./setup --team
> ```
> Then restart your AI coding tool.

Do not skip skills, ignore gstack errors, or work around missing gstack.

Use the `/browse` skill from gstack for all web browsing. Never use `mcp__Claude_in_Chrome__*` tools directly.
Use `~/.claude/skills/gstack/...` for gstack file paths (the global path).

Available gstack skills:
- `/office-hours` — collaborative problem-solving session
- `/plan-ceo-review` — prepare a plan for CEO review
- `/plan-eng-review` — prepare a plan for engineering review
- `/plan-design-review` — prepare a plan for design review
- `/design-consultation` — design consultation session
- `/design-shotgun` — rapid design exploration
- `/design-html` — generate HTML designs
- `/review` — code review
- `/ship` — ship a feature end-to-end
- `/land-and-deploy` — land and deploy changes
- `/canary` — canary deployment
- `/benchmark` — run benchmarks
- `/browse` — web browsing (use this for ALL web browsing)
- `/connect-chrome` — connect to Chrome browser
- `/qa` — full QA pass
- `/qa-only` — QA without implementation
- `/design-review` — design review session
- `/setup-browser-cookies` — set up browser cookies
- `/setup-deploy` — set up deployment
- `/setup-gbrain` — set up gbrain
- `/retro` — retrospective
- `/investigate` — investigate an issue
- `/document-release` — document a release
- `/codex` — codex tasks
- `/cso` — CSO tasks
- `/autoplan` — automatic planning
- `/plan-devex-review` — prepare a plan for devex review
- `/devex-review` — developer experience review
- `/careful` — careful/cautious mode
- `/freeze` — freeze a feature or branch
- `/guard` — guard mode
- `/unfreeze` — unfreeze a feature or branch
- `/gstack-upgrade` — upgrade gstack
- `/learn` — learning session
