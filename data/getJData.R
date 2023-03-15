getJData <- function(game.ids) {

  require(stringr)
  require(magrittr)
  
  #initialize output df
  row.ct <- 61*length(game.ids)
  df.out <- data.frame(
    game.id = rep(game.ids, each = 61),
    ind = integer(row.ct),
    category = character(row.ct),
    value = integer(row.ct),
    clue = character(row.ct),
    response = character(row.ct),
    is.dd = integer(row.ct)
  )
  
  for (i in 1:length(game.ids)) {
    
    game.id <- game.ids[i]
  
    #pull page
    page <- readLines(paste0('https://www.j-archive.com/showgame.php?game_id=',game.id), encoding = 'UTF-8') %>%
      suppressWarnings %>%
      as.character
    
    #get categories & clean
    cats <- page %>%
      `[`(grep("<tr><td class=\"category_name\">", .)) %>% 
      stringr::str_sub(start = 33, end = -11) %>%
      gsub(pattern='<em class=\\"underline\\">', replacement='') %>%
      gsub(pattern="</em>", replacement='') %>%
      gsub(pattern="&quot;", replacement="'") %>%
      gsub(pattern="\\\\", replacement='') %>%
      gsub(pattern="&amp;", replacement='&') %>%
      gsub(pattern='<a href="https://www.j-archive.com/media/', replacement='<') %>%
      gsub(pattern='<a href="http://www.j-archive.com/media/', replacement='<') %>%
      gsub(pattern="</a>", replacement='') %>%
      gsub(pattern='"', replacement="'") %>%
      gsub(pattern="<[^>]*>", replacement="")
    
    #FJ category line has a couple extra characters
    cats[length(cats)] %<>%
      stringr::str_sub(start = 3)
    
    #values to build grid
    values <- c(200,400,600,800,1000,
                400,800,1200,1600,2000)
    
    #create grid to fill in with clue/response/dd
    df.grid <- data.frame(
      ind = 1:61,
      game.id = game.id,
      round = c(rep('JJ',30), rep('DJ',30), 'FJ'),
      category = c(rep(cats[1:6],5),rep(cats[7:12],5),cats[13]),
      value = c(rep(values,each=6),0)
    )
  
    #get line numbers that have clue/response info
    line.nbrs <- page %>% 
      grep(pattern = "<div onmouseover")
    data.lines <- page[line.nbrs]
    
    #determine which clues have info
    data.locs <- data.lines %>%
      `[`(1:length(.)-1) %>%
      stringr::str_sub(
        start = stringr::str_locate(., 'clue_')[,2] + 1,
        end = stringr::str_locate(., 'clue_')[,2] + 6
      ) %>%
      gsub(pattern="'", replacement="") %>%
      data.table::tstrsplit('_') %>%
      as.data.frame
    index <- c(ifelse(data.locs[,1] == 'DJ', 30, 0) +
                 as.numeric(data.locs[,3])*6 - 6 +
                 as.numeric(data.locs[,2]),
               61) #FJ
    
    #get clues & clean
    clues <-
      data.lines %>%
      stringr::str_sub(
        start = stringr::str_locate_all(., '_stuck') %>%
          sapply(function(x) `[`(x,2,1)) + 10,
        end = stringr::str_locate(., 'onclick')[,1] - 5
      ) %>%
      gsub(pattern="&quot;", replacement="'") %>%
      gsub(pattern="\\\\", replacement='') %>%
      gsub(pattern="&amp;", replacement='&') %>%
      gsub(pattern="&lt;a href='https://www.j-archive.com/media/", replacement='<') %>%
      gsub(pattern="&lt;a href='http://www.j-archive.com/media/", replacement='<') %>%
      gsub(pattern="' target='_blank'&gt;", replacement='> ')%>%
      gsub(pattern="'&gt;", replacement='> ') %>%
      gsub(pattern="&lt;/a&gt;", replacement='') %>%
      gsub(pattern="&lt;br /&gt;", replacement='') %>%
      gsub(pattern="&lt;i&gt;", replacement="'") %>%
      gsub(pattern="&lt;/i&gt;", replacement="'") %>%
      gsub(pattern="&lt;sup&gt;", replacement="(") %>%
      gsub(pattern="&lt;/sup&gt;", replacement=")") %>%
      gsub(pattern="&lt;sub&gt;", replacement="(") %>%
      gsub(pattern="&lt;/sub&gt;", replacement=")") %>%
      gsub(pattern="&lt;b&gt;", replacement="") %>%
      gsub(pattern="&lt;/b&gt;", replacement="") %>%
      gsub(pattern='"', replacement="'")
    
    #get responses & clean
    responses <-
      data.lines %>%
      stringr::str_sub(
        start = stringr::str_locate(., 'correct_response')[,2] + 11,
        end = stringr::str_locate(., '/em&gt;')[,1] - 5
      ) %>%
    gsub(pattern="&quot;", replacement="'") %>%
      gsub(pattern="\\\\", replacement='') %>%
      gsub(pattern="&amp;", replacement='&') %>%
      gsub(pattern="&lt;i&gt;", replacement="'") %>%
      gsub(pattern="&lt;/i&gt;", replacement="'") %>%
      gsub(pattern='"', replacement="'")
    
    #initialize daily double indicator
    dd <- rep(0,length(index))
    #get line numbers that indicate daily double
    dd.nbrs <- page %>% 
      grep(pattern = "<td class=\"clue_value_daily_double")
    #find which clues the daily doubles apply to
    dd.locs <- dd.nbrs %>%
      sapply(function(x) max(which(line.nbrs < x)))
    #change indicator to 1 for these clues
    dd[dd.locs] <- 1
    
    #build game object
    df.game <- df.grid %>%
      dplyr::left_join(
        data.frame(
          ind = index,
          clue = clues,
          reponse = responses,
          is.dd = dd
        ),
        on = ind
      )
    
    #write to output object
    df.out[(61*(i-1)+1):(61*i),] <- df.game[,-1]
  
  }
  
  #return
  df.out

}

jData <- getJData(ids.all[ids.all$season %in% 34:39,2])
write.table(jData, file="~/JeopBoardy/data/jData.txt", sep="|", quote = 1)
write.csv(jData, file="~/JeopBoardy/data/jData.csv")
